using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteBookObj;

namespace NoteAppUTProj
{
    [TestClass]
    public class NoteBookSerializerUT
    {
        //dependecy injection
        private NoteList allNotes = new NoteList();
        private NoteList displayNotes = new NoteList();

        private NoteListSerializer serializer = new NoteListSerializer();
        private const String NOTES_FILENAME = "notes.bin";
        private const String NOTES_BACKUP_FILENAME = "notes.bak";


        [TestMethod]
        public void TestCreateNote()
        {
            try
            {
               allNotes.Add(new Note());
            }
            catch(Exception ex)
            {
                StringAssert.Contains(ex.Message, "out of memory");
            }

        }
        [TestMethod]
        public void TestCreateNotesInLoop()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    allNotes.Add(new Note());
                }
            }
            catch (Exception ex)
            {
                StringAssert.Contains(ex.Message, "out of memory");
            }

        }
        [TestMethod]
        public void TestEditNote()
        {
        }
        [TestMethod]
        public void TestDeleteNote()
        {
        }
        [TestMethod]
        public void TestSyncNotes()
        {
        }
    }
}
