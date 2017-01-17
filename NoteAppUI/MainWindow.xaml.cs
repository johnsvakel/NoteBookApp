using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NoteBookObj;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NoteAppUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataGridViewColumn previewTextCol;
        private DataGridViewColumn modifiedTextCol;
        private NoteBookObj.NoteList allNotes = new NoteList();
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
        public MainWindow()
        {
            InitializeComponent();
            InitDataGrid();
            //SetTabWidth(richTextBoxNotes, TAB_WIDTH);

            LoadNotes();
            RefreshDisplayNotes();
        }
         private void InitDataGrid()
        {
            dataGridView1.ItemsSource = displayNotes;
            //// Some shorthand references
            //previewTextCol = dataGridView1.Columns["textColumn"];
            //modifiedTextCol = dataGridView1.Columns["modifiedDateColumn"];

            //dataGridView1.AutoGenerateColumns = false;
            //dataGridView1.DataSource = displayNotes;

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
          private void RefreshDisplayNotes()
        {
            // Save the selected row index
            int selectedRowIdx = -1;

            selectedRowIdx = dataGridView1.SelectedIndex;

            // Refresh the list
            displayNotes.Clear();

            String searchText = textBoxSearch.Text;

            foreach (Note n in allNotes)
            {
                if (n.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    displayNotes.Add(n);
            }

            // Restore the selected row, or close to it if possible
            if (selectedRowIdx == dataGridView1.Items.Count)
                selectedRowIdx--;

            foreach (DataGridViewRow row in dataGridView1.Items)
            {
                if (row.Index == selectedRowIdx)
                    row.Selected = true;
            }
        }
         [DllImport("User32.dll", CharSet = CharSet.Auto)]
         public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);

         //public static void SetTabWidth(RichTextBox textBox, int tabWidth)
         //{
         //    Graphics g = textBox.CreateGraphics();
         //   // SendMessage(textBox.Handle, EM_SETTABSTOPS, 1, new int[] { tabWidth * FONT_TO_TEMPLATE_WIDTH_RATIO });
         //}


        private void buttonNewNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonDeleteNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEmailNote_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
