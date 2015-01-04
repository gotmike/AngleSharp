﻿using AngleSharp;
using AngleSharp.DOM;
using NUnit.Framework;

namespace UnitTests.Html
{
    [TestFixture]
    public class DomManipulation
    {
        public IDocument CreateDocument()
        {
            var doc = DocumentBuilder.Html("");
            doc.RemoveChild(doc.DocumentElement);
            return doc;
        }

        [Test]
        public void ChildlessDocument()
        {
            var doc = CreateDocument();
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void ChildlessHtmlElement()
        {
            var doc = CreateDocument();
            doc.AppendChild(doc.CreateElement("html"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void BodyFollowedByFramesetInsideTheHtmlElement()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("html"));
            var b = html.AppendChild(doc.CreateElement("body"));
            html.AppendChild(doc.CreateElement("frameset"));
            Assert.AreEqual(b, doc.Body);
        }

        [Test]
        public void FramesetFollowedByBodyInsideTheHtmlElement()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("html"));
            var f = html.AppendChild(doc.CreateElement("frameset"));
            html.AppendChild(doc.CreateElement("body"));
            Assert.AreEqual(f, doc.Body);
        }

        [Test]
        public void BodyFollowedByFramesetInsideAnonHtmlElement()
        {
            var doc = CreateDocument();
            var html =
              doc.AppendChild(doc.CreateElement("http://example.org/test", "html"));
            html.AppendChild(doc.CreateElement("body"));
            html.AppendChild(doc.CreateElement("frameset"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void FramesetFollowedByBodyInsideAnonHtmlElement()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("http://example.org/test", "html"));
            html.AppendChild(doc.CreateElement("frameset"));
            html.AppendChild(doc.CreateElement("body"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void NonHtmlBodyFollowedByBodyInsideTheHtmlElement()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("html"));
            html.AppendChild(doc.CreateElement("http://example.org/test", "body"));
            var b = html.AppendChild(doc.CreateElement("body"));
            Assert.AreEqual(b, doc.Body);
        }

        [Test]
        public void NonHtmlFramesetFollowedByBodyInsideTheHtmlElement()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("html"));
            html.AppendChild(doc.CreateElement("http://example.org/test", "frameset"));
            var b = html.AppendChild(doc.CreateElement("body"));
            Assert.AreEqual(b, doc.Body);
        }

        [Test]
        public void BodyInsideAnxElementFollowedByBody()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("html"));
            var x = html.AppendChild(doc.CreateElement("x"));
            x.AppendChild(doc.CreateElement("body"));
            var body = html.AppendChild(doc.CreateElement("body"));
            Assert.AreEqual(body, doc.Body);
        }

        [Test]
        public void FramesetInsideAnXElementFollowedByFrameset()
        {
            var doc = CreateDocument();
            var html = doc.AppendChild(doc.CreateElement("html"));
            var x = html.AppendChild(doc.CreateElement("x"));
            x.AppendChild(doc.CreateElement("frameset"));
            var frameset = html.AppendChild(doc.CreateElement("frameset"));
            Assert.AreEqual(frameset, doc.Body);
        }

        [Test]
        public void BodyAsTheRootNode()
        {
            var doc = CreateDocument();
            doc.AppendChild(doc.CreateElement("body"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void FramesetAsTheRootNode()
        {
            var doc = CreateDocument();
            doc.AppendChild(doc.CreateElement("frameset"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void BodyAsTheRootNodeWithAFramesetChild()
        {
            var doc = CreateDocument();
            var body = doc.AppendChild(doc.CreateElement("body"));
            body.AppendChild(doc.CreateElement("frameset"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void FramesetAsTheRootNodeWithABodyChild()
        {
            var doc = CreateDocument();
            var frameset = doc.AppendChild(doc.CreateElement("frameset"));
            frameset.AppendChild(doc.CreateElement("body"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void NonHtmlBodyAsTheRootNode()
        {
            var doc = CreateDocument();
            doc.AppendChild(doc.CreateElement("http://example.org/test", "body"));
            Assert.IsNull(doc.Body);
        }

        [Test]
        public void NonHtmlFramesetAsTheRootNode()
        {
            var doc = CreateDocument();
            doc.AppendChild(doc.CreateElement("http://example.org/test", "frameset"));
            Assert.IsNull(doc.Body);
        }
    }
}
