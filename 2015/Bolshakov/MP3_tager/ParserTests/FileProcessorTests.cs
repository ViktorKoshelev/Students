﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mp3Handler;

namespace Tests
{
    [TestClass]
    public class FileProcessorTests
    {
        [TestMethod]
        public void SimpleAddTagsToCleanFile()
        {
            var fileHandler = new TestFileHandler("myartist - mysong");
            var fileProcesor = new Mp3FileProcessor(fileHandler);

            var expect = new Dictionary<FrameType, string>
            {
                {FrameType.Artist, "myartist"},
                {FrameType.Title, "mysong"}
            };

            Assert.AreEqual(true,fileProcesor.RetagFile("<ar> - <ti>"));

            CollectionAssert.AreEquivalent(expect,fileHandler.Tags);
        }

        [TestMethod]
        public void SimpleAddTagsToNotEmptyFile()
        {
            var fileHandler = new TestFileHandler("myartist - mysong");
            fileHandler.Tags.Add(FrameType.Artist, "Not my artist");
            var fileProcesor = new Mp3FileProcessor(fileHandler);

            var expect = new Dictionary<FrameType, string>
            {
                {FrameType.Artist, "myartist"},
                {FrameType.Title, "mysong"}
            };

            Assert.AreEqual(true, fileProcesor.RetagFile("<ar> - <ti>"));

            CollectionAssert.AreEquivalent(expect, fileHandler.Tags);
        }

        [TestMethod]
        public void NotCorrectPatternAddTags()
        {
            var fileHandler = new TestFileHandler("myartist - mysong");
            var fileProcesor = new Mp3FileProcessor(fileHandler);

            Assert.AreEqual(false, fileProcesor.RetagFile("<ar><ti>"));
        }

        [TestMethod]
        public void RenameFile()
        {
            var fileHandler = new TestFileHandler("my name")
            {
                Tags = new Dictionary<FrameType, string>()
                {
                    {FrameType.Artist, "myartist"},
                    {FrameType.Title, "mysong"}
                }
            };
            var fileProcesor = new Mp3FileProcessor(fileHandler);

            Assert.AreEqual(true, fileProcesor.RenameFile("<ar> - <ti>"));

            Assert.AreEqual("myartist - mysong", fileHandler.FileName);
        }

        [TestMethod]
        public void NotCorrectPatternRenameFile()
        {
            var fileHandler = new TestFileHandler("my name")
            {
                Tags = new Dictionary<FrameType, string>()
                {
                    {FrameType.Artist, "myartist"},
                    {FrameType.Title, "mysong"}
                }
            };
            var fileProcesor = new Mp3FileProcessor(fileHandler);

            Assert.AreEqual(false, fileProcesor.RenameFile("<a> - <ti>"));
        }

        [TestMethod]
        public void DifferenceTest()
        {
            var fileHandler = new TestFileHandler("not my artist - my song")
            {
                Tags = new Dictionary<FrameType, string>()
                {
                    {FrameType.Artist, "my artist"},
                    {FrameType.Title, "my song"}
                }
            };
            var fileProcesor = new Mp3FileProcessor(fileHandler);

            var fileExpect = "my artist";
            var pathExpect = "not my artist";

            var result = fileProcesor.Difference("<ar> - <ti>");

            Assert.AreEqual(1,result.Count);
            Assert.AreEqual(fileExpect,result[FrameType.Artist].FileValue);
            Assert.AreEqual(pathExpect,result[FrameType.Artist].PathValue);
        }
    }
}
