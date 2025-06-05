using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTestsTemplateProcessor
    {
        [TestMethod]
        public void TemplateProcessor_TestTemplates()
        {
            MKVTools.TemplateProcessor tp = new MKVTools.TemplateProcessor();
            MKVTools.MakeMKV makeMKV = new MKVTools.MakeMKV();
            MKVTools.Disc disc = new MKVTools.Disc(MKVTools.DiscType.Bluray, "The Iron Giant: Signature Edition", MKVTools.Language.English, null);
            MKVTools.MKVToolNix mkvToolNix = new MKVTools.MKVToolNix();

            string scanInfo = Properties.Resources.TheIronGiant_title1;
            List<string> scanInfoArray = new List<string>(scanInfo.Split(new string[] { "\\r\\n" }, StringSplitOptions.RemoveEmptyEntries));

            MKVTools.Source src = new MKVTools.Source(makeMKV, MKVTools.SourceType.File, @"\\fenrir.adeptweb.dk\Incoming\!BD\!ToMKV\The Iron Giant (1999)", disc, scanInfoArray, mkvToolNix);
            MKVTools.Title title = src.Titles[0];

            string[] templates = {
                "{source-file-clean}",
                "{source-file-clean}{ [BD]|source-isbluray}{ [DVD]|source-isdvd} {[3D]|video-3d||[2D]}{ [|video-resolution|]}",
                "{source-file} ({video-codec}, {video-framerate} fps)",
                "{name-default}",
                "{filename-default}"
            };
            string[] expected = {
                "The Iron Giant (1999)",
                "The Iron Giant (1999) [BD] [2D] [1920x1080]",
                "The Iron Giant (1999) (MPEG4, 24 fps)",
                "The Iron Giant: Signature Edition",
                "The_Iron_Giant_Signature_Edition_t01"
            };
            string result;
            int n = templates.Length;

            for (int i = 0; i < n; i++)
            {
                result = MKVTools.TemplateProcessor.GetProcessedName(title, src, templates[i]);
                Assert.AreEqual<string>(expected[i], result, string.Format("'{0}': result not as expected.", templates[i]));
            }
        }
    }
}
