﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mp3TagLib;

namespace Mp3TagTest
{
    [TestClass]
    public class AnalyzerTestClass
    {
        private Analyzer _analyzerWithFiltr;
        private Analyzer _analyzerWithoutFiltr;
        private string[] _testPaths;
        private string _expectedSynchronizedFile;
        private string _expectedNotSynchronizedFile;
        private string _expectedErrorFile;
        private int _expectedSynchronizedFilesCount;
        private int _expectedNotSynchronizedFilesCount;

        [TestInitialize]
        public void Init()
        {
            _analyzerWithoutFiltr = new Analyzer(new Tager(new TestFileLoader()));
            _analyzerWithFiltr = new Analyzer(new Tager(new TestFileLoader()), str => !str.Contains(".UnknownExtension"));
            _expectedSynchronizedFile = "testArtist testAlbum";
            _expectedNotSynchronizedFile = "testArtist testGenre";
            _expectedErrorFile = "test1.UnknownExtension";
            _expectedSynchronizedFilesCount = 1;
            _expectedNotSynchronizedFilesCount = 1;
            _testPaths = new string[] { _expectedErrorFile, _expectedSynchronizedFile, _expectedNotSynchronizedFile };
        }

        [TestMethod]
        public void SynchronizedFilesContainsOnlyExpectedFile()
        {
            _analyzerWithFiltr.Analyze(_testPaths, new Mask("{artist} {album}"));
           
            Assert.AreEqual(_expectedSynchronizedFile, _analyzerWithFiltr.SynchronizedFiles.First().Name);
            Assert.AreEqual(_expectedSynchronizedFilesCount, _analyzerWithFiltr.SynchronizedFiles.Count);
        }

        [TestMethod]
        public void NotSynchronizedFilesContainsExpectedFile()
        {
            _analyzerWithFiltr.Analyze(_testPaths, new Mask("{artist} {album}"));
           
            Assert.AreEqual(_expectedNotSynchronizedFilesCount, _analyzerWithFiltr.NotSynchronizedFiles.Count);
            Assert.AreEqual(_expectedNotSynchronizedFile, _analyzerWithFiltr.NotSynchronizedFiles.Keys.First().Name);
        }

        [TestMethod]
        public void IfAnalyzerWithFiltrErrorFilesIsEmpty()
        {
            _analyzerWithFiltr.Analyze(_testPaths, new Mask("{artist} {album}"));
            
            Assert.AreEqual(0,_analyzerWithFiltr.ErrorFiles.Count);
        }

        [TestMethod]
        public void IfAnalyzerWithoutFiltrErrorFilesContainsErrorFile()
        {
            _analyzerWithoutFiltr.Analyze(_testPaths, new Mask("{artist} {album}"));
           
            Assert.AreEqual(1, _analyzerWithoutFiltr.ErrorFiles.Count);
        }
    }
}
