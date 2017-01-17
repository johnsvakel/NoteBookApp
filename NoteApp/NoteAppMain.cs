using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NoteBookObj;
using NoteBookSync;

namespace NoteApp
{
    public partial class NoteAppMain : Form
    {
        private NoteAppPreference objPreference = new NoteAppPreference();
        private DataGridViewColumn previewTextCol;
        private DataGridViewColumn modifiedTextCol;
        private NoteList allNotes = new NoteList();
        private NoteList displayNotes = new NoteList();
        private int selectedNoteIdx = -1; // Index of selected note in displayNotes
        private Note selectedNote;
        private NoteListSerializer serializer = new NoteListSerializer();
        private const String NOTES_FILENAME = "notes.bin";
        private const String NOTES_BACKUP_FILENAME = "notes.bak";

        private const int EM_SETTABSTOPS = 0x00CB;
        private const int FONT_TO_TEMPLATE_WIDTH_RATIO = 4;
        private const int TAB_WIDTH = 4;

        private const int TIME_BETWEEN_SAVES = 60000; 

        private Timer saveTimer = new Timer();
        public NoteAppMain()
        {
            InitializeComponent();
            InitDataGrid();
            SetTabWidth(richTextBoxNotes, TAB_WIDTH);

            LoadNotes();
            RefreshDisplayNotes();
        }
        private void buttonNewNote_Click(object sender, EventArgs e)
        {
            NewNote();
        }
        private void InitDataGrid()
        {
            // Some shorthand references
            previewTextCol = dataGridView1.Columns["textColumn"];
            modifiedTextCol = dataGridView1.Columns["modifiedDateColumn"];

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = displayNotes;

            // Tell the datagrid which properties of a Note object to use in which columns
            previewTextCol.DataPropertyName = "PreviewText";
            modifiedTextCol.DataPropertyName = "ModifiedText";

            modifiedTextCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void LoadNotes()
        {            
            allNotes = serializer.DeserializeObject(NOTES_FILENAME);
            if (allNotes == null)
                allNotes = new NoteList();
        }
        private void NewNote()
        {
            allNotes.Add(new Note());

            RefreshDisplayNotes();
        }
        private void RefreshDisplayNotes()
        {
            // Save the selected row index
            int selectedRowIdx = -1;
            if (dataGridView1.SelectedRows.Count > 0)
                selectedRowIdx = dataGridView1.SelectedRows[0].Index;

            // Refresh the list
            displayNotes.Clear();

            String searchText = textBoxSearch.Text;

            foreach (Note n in allNotes)
            {
                if (n.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    displayNotes.Add(n);
            }

            // Restore the selected row, or close to it if possible
            if (selectedRowIdx == dataGridView1.RowCount)
                selectedRowIdx--;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Index == selectedRowIdx)
                    row.Selected = true;
            }
        }
        private void InitSaveTimer()
        {
            saveTimer.Interval = TIME_BETWEEN_SAVES;
            saveTimer.Tick += new System.EventHandler(SaveTimerTick);
            saveTimer.Enabled = true;

            ResetSaveTimer();
        }

        private void ResetSaveTimer()
        {
            saveTimer.Stop();
            saveTimer.Start();
        }

        private void SaveTimerTick(Object sender, EventArgs e)
        {
            SaveAllNotes();
        }
         private void ChangeSelectedNote(int displayNotesIdx)
        {
            selectedNoteIdx = displayNotesIdx;
            selectedNote = null;

            if (selectedNoteIdx < 0)
            {
                richTextBoxNotes.Text = "";
                labelCreated.Text = "";
                labelEdited.Text = "";
                return;
            }

            selectedNote = displayNotes[selectedNoteIdx];

            richTextBoxNotes.Text = selectedNote.Text;

            labelCreated.Text = "Note created ";
            if (selectedNote.Created.Date == DateTime.Today)
                labelCreated.Text += "at " + selectedNote.CreatedText;
            else
                labelCreated.Text += "on " + selectedNote.CreatedText;

            labelEdited.Text = "Last saved ";
            if (selectedNote.Modified.Date == DateTime.Today)
                labelEdited.Text += "at " + selectedNote.ModifiedText;
            else
                labelEdited.Text += "on " + selectedNote.ModifiedText;
        }
        private bool SaveAllNotes()
        {
            if (serializer.SerializeObject(NOTES_FILENAME, allNotes))
                return true;

            return false;
        }
        private bool BackupNotes()
        {
            if (SaveAllNotes() && serializer.SerializeObject(NOTES_BACKUP_FILENAME, allNotes))
                return true;

            return false;
        }
         private void deleteButton_Click(object sender, EventArgs e)
        {
            allNotes.Remove(selectedNote);

            selectedNote = null;
            selectedNoteIdx = -1;

            RefreshDisplayNotes();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            RefreshDisplayNotes();
        }
        private void notesDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void notesDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            int noteIdx = dataGridView1.SelectedRows.Count == 1 ? dataGridView1.SelectedRows[0].Index : -1;
            ChangeSelectedNote(noteIdx);
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);

        public static void SetTabWidth(RichTextBox textBox, int tabWidth)
        {
            Graphics g = textBox.CreateGraphics();

            SendMessage(textBox.Handle, EM_SETTABSTOPS, 1, new int[] { tabWidth * FONT_TO_TEMPLATE_WIDTH_RATIO });
        }

        private void buttonDeleteNote_Click(object sender, EventArgs e)
        {
            allNotes.Remove(selectedNote);
            selectedNote = null;
            selectedNoteIdx = -1;
            RefreshDisplayNotes();
        }

        private void Note_TextChanged(object sender, EventArgs e)
        {
            if (selectedNote != null)
                selectedNote.Text = richTextBoxNotes.Text;
            // Redraw the note list to show updated previews
            dataGridView1.Invalidate();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshDisplayNotes();
        }

        private void NoteAppMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!BackupNotes())
            {
                e.Cancel = true;
                MessageBox.Show("For some reason your notes could not be saved. You may try again, or move important notes to another program to ensure they are not lost.",
                                "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSync_Click(object sender, EventArgs e)
        {
            SyncNotes();
        }
        private void SyncNotes()
        {
            NoteBookSync.NoteBookBackGroudSync obj = new NoteBookBackGroudSync();
            obj.NoteBookBackGroudSyncCreate(objPreference.GetLocalServer(), objPreference.GetLocalServer());
        }

        private void serverSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferenceDlg notebookPref = new PreferenceDlg();
            notebookPref.SetPreference(objPreference);
            notebookPref.Show();
        }
    }
}
