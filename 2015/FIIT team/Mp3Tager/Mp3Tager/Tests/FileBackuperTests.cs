﻿/*
using System.IO;
using FileLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Fakes;

namespace Tests
{
    [TestClass]
    public class FileBackuperTests
    {
        [TestMethod]
        public void Backup_Successful()
        {
            var backup = new FileBackuper(new FakeMp3File(new Mp3Tags(), @"D:\music\audioq.mp3", new FakeFileExistenceChecker()));
            Assert.AreEqual(backup.TempFile.FullName, Path.GetTempPath() + @"audioq.mp3");

            backup.RestoreFromBackup();
            Assert.AreEqual(backup.TempFile.FullName, @"D:\music\audioq.mp3");
        }
    }
}
*/
