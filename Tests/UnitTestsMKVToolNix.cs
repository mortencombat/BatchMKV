using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class UnitTestsMKVToolNix
    {
        string[] desc = { "v17.0.0 64-bit", "v18.0.0 64-bit", "v19.0.0 64-bit", "v20.0.0 64-bit" };
        string[] path = { "MKVToolNix17", "MKVToolNix18", "MKVToolNix19", "MKVToolNix20" };
        string pathBase = @"D:\Kenneth\Dropbox\Adeptweb\Windows Development\BatchMKV\Tests\Data\Tools\";
        Version[] version = { new Version(17, 0, 0), new Version(18, 0, 0), new Version(19, 0, 0), new Version(20, 0, 0) };
        bool[] version64bit = { true, true, true, true };

        [TestMethod]
        public void MKVToolNix_TestVersionResolver()
        {
            int n = desc.GetLength(0);

            MKVTools.MKVToolNix mkvToolNix = new MKVTools.MKVToolNix();
            Version versionActual;

            for (int i = 0; i < n; i++)
            {
                mkvToolNix.MKVToolNixPath = pathBase + path[i];

                // Check 32/64-bit
                Assert.AreEqual<bool>(version64bit[i], mkvToolNix.Version64Bit, string.Format("{0}: is 64-bit not resolved as expected.", desc[i]));

                // Check version
                versionActual = mkvToolNix.Version;
                Assert.AreEqual<int>(version[i].Major, versionActual.Major, string.Format("{0}: major version not resolved as expected.", desc[i]));
                Assert.AreEqual<int>(version[i].Minor, versionActual.Minor, string.Format("{0}: minor version not resolved as expected.", desc[i]));
                Assert.AreEqual<int>(version[i].Build, versionActual.Build, string.Format("{0}: build version not resolved as expected.", desc[i]));
            }
        }

        string[] mkvFiles = { "nofile.mkv", "S01 Inside Breaking Bad - 'Acting' with Cerebral Palsy.mkv", "S05E09 Talking Bad S01E01.mkv", "Disc 1.mkv" };
        string mkvFilePath = @"D:\Kenneth\Dropbox\Adeptweb\Windows Development\BatchMKV\Tests\Data\Videos\";
        MKVTools.MKVFile.OperationResult[] resultExpected = { MKVTools.MKVFile.OperationResult.MKVInfoError, MKVTools.MKVFile.OperationResult.FileOpened, MKVTools.MKVFile.OperationResult.FileOpened, MKVTools.MKVFile.OperationResult.FileOpened };
        int[] trackCountExpected = { 0, 2, 2, 5 };
        MKVTools.TrackType[][] trackTypeExpected = { null, new MKVTools.TrackType[] { MKVTools.TrackType.Video, MKVTools.TrackType.Audio }, new MKVTools.TrackType[] { MKVTools.TrackType.Video, MKVTools.TrackType.Audio }, new MKVTools.TrackType[] { MKVTools.TrackType.Video, MKVTools.TrackType.Audio, MKVTools.TrackType.Audio, MKVTools.TrackType.Subtitle, MKVTools.TrackType.Subtitle } };
        string[][] trackCodecIDExpected = { null, new string[] { "V_MPEG2", "A_AC3" }, new string[] { "V_MPEG4/ISO/AVC", "A_AC3" }, new string[] { "V_MPEG4/ISO/AVC", "A_AC3", "A_AC3", "S_HDMV/PGS", "S_HDMV/PGS" } };
        MKVTools.Language[][] trackLanguageExpected = { null, new MKVTools.Language[] { MKVTools.Language.English, MKVTools.Language.English }, new MKVTools.Language[] { MKVTools.Language.English, MKVTools.Language.English }, new MKVTools.Language[] { MKVTools.Language.Undetermined, MKVTools.Language.English, MKVTools.Language.English, MKVTools.Language.English, MKVTools.Language.English } };
        bool[][] trackDefaultExpected = { null, new bool[] { false, false }, new bool[] { true, true }, new bool[] { false, true, false, false, false } };
        bool[][] trackEnabledExpected = { null, new bool[] { true, true }, new bool[] { true, true }, new bool[] { true, true, false, true, true } };
        bool[][] trackForcedExpected = { null, new bool[] { false, false }, new bool[] { false, false }, new bool[] { false, false, false, false, false } };

        [TestMethod]
        public void MKVToolNix_TestOpen()
        {
            int n = desc.GetLength(0);
            int m = mkvFiles.GetLength(0);
            int o;

            MKVTools.MKVToolNix mkvToolNix = new MKVTools.MKVToolNix();
            MKVTools.MKVFile mkvFile = new MKVTools.MKVFile(mkvToolNix);
            MKVTools.MKVFile.OperationResult resultActual;
            MKVTools.MKVFile.Track tr;

            for (int i = 0; i < n; i++)
            {
                mkvToolNix.MKVToolNixPath = pathBase + path[i];
                Assert.IsTrue(mkvToolNix.Available, string.Format("{0}: not available.", desc[i]));

                for (int j = 0; j < m; j++)
                {
                    resultActual = mkvFile.Open(mkvFilePath + mkvFiles[j]);
                    Assert.AreEqual<MKVTools.MKVFile.OperationResult>(resultExpected[j], resultActual, string.Format("{0} ({1}): MKVFile.Open() not as expected.", desc[i], mkvFiles[j]));

                    o = trackCountExpected[j];
                    Assert.AreEqual<int>(o, (mkvFile.Tracks != null ? mkvFile.Tracks.Length : 0), string.Format("{0} ({1}): Track count not as expected.", desc[i], mkvFiles[j]));

                    for (int k = 0; k < o; k++)
                    {
                        tr = mkvFile.Tracks[k];
                        Assert.AreEqual<MKVTools.TrackType>(trackTypeExpected[j][k], tr.Type, string.Format("{0} ({1}, track {2} of {3}): Track type not as expected.", desc[i], mkvFiles[j], k + 1, o));
                        Assert.AreEqual<string>(trackCodecIDExpected[j][k], tr.CodecID, string.Format("{0} ({1}, track {2} of {3}): Track codec ID not as expected.", desc[i], mkvFiles[j], k + 1, o));
                        Assert.AreEqual<bool>(trackDefaultExpected[j][k], tr.Default, string.Format("{0} ({1}, track {2} of {3}): Track default flag not as expected.", desc[i], mkvFiles[j], k + 1, o));
                        Assert.AreEqual<bool>(trackEnabledExpected[j][k], tr.Enabled, string.Format("{0} ({1}, track {2} of {3}): Track enabled flag not as expected.", desc[i], mkvFiles[j], k + 1, o));
                        Assert.AreEqual<bool>(trackForcedExpected[j][k], tr.Forced, string.Format("{0} ({1}, track {2} of {3}): Track forced flag not as expected.", desc[i], mkvFiles[j], k + 1, o));
                    }
                }
            }
        }

        [TestMethod]
        public void MKVToolNix_TestModify()
        {
            int n = desc.GetLength(0);
            int m = mkvFiles.GetLength(0);

            MKVTools.MKVToolNix mkvToolNix = new MKVTools.MKVToolNix();
            MKVTools.MKVFile mkvFile = new MKVTools.MKVFile(mkvToolNix);
            MKVTools.MKVFile.OperationResult resultActual;

            string titleName, trackName;
            MKVTools.Language trackLanguage;

            for (int i = 0; i < n; i++)
            {
                mkvToolNix.MKVToolNixPath = pathBase + path[i];
                Assert.IsTrue(mkvToolNix.Available, string.Format("{0}: not available.", desc[i]));

                // for (int j = 0; j < m; j++)
                for (int j = 0; j < m; j++)
                {
                    resultActual = mkvFile.Open(mkvFilePath + mkvFiles[j]);
                    Assert.AreEqual<MKVTools.MKVFile.OperationResult>(resultExpected[j], resultActual, string.Format("{0} ({1}): MKVFile.Open() not as expected.", desc[i], mkvFiles[j]));

                    if (mkvFile.Tracks != null && mkvFile.Tracks.Length > 0)
                    {
                        titleName = mkvFile.Title;
                        trackName = mkvFile.Tracks[0].Name;
                        trackLanguage = mkvFile.Tracks[0].Language;

                        mkvFile.Title = "test title";
                        mkvFile.Tracks[0].Name = "test name";
                        mkvFile.Tracks[0].Language = MKVTools.Language.Danish;

                        resultActual = mkvFile.Save();
                        Assert.AreEqual<MKVTools.MKVFile.OperationResult>(MKVTools.MKVFile.OperationResult.FileSaved, resultActual, string.Format("{0} ({1}): MKVFile.Save() not as expected.", desc[i], mkvFiles[j]));

                        // Re-open file and check that changes have been applied.
                        resultActual = mkvFile.Open(mkvFilePath + mkvFiles[j]);
                        Assert.AreEqual<MKVTools.MKVFile.OperationResult>(resultExpected[j], resultActual, string.Format("{0} ({1}): MKVFile.Open() not as expected (after MKVPropEdit).", desc[i], mkvFiles[j]));

                        Assert.AreEqual<string>("test title", mkvFile.Title, string.Format("{0} ({1}): Title not as expected after MKVPropEdit.", desc[i], mkvFiles[j]));
                        Assert.AreEqual<string>("test name", mkvFile.Tracks[0].Name, string.Format("{0} ({1}): Track name not as expected after MKVPropEdit.", desc[i], mkvFiles[j]));
                        Assert.AreEqual<MKVTools.Language>(MKVTools.Language.Danish, mkvFile.Tracks[0].Language, string.Format("{0} ({1}): Track language not as expected after MKVPropEdit.", desc[i], mkvFiles[j]));

                        // Re-set to previous values.
                        mkvFile.Title = titleName;
                        mkvFile.Tracks[0].Name = trackName;
                        mkvFile.Tracks[0].Language = trackLanguage;
                        resultActual = mkvFile.Save();
                        Assert.AreEqual<MKVTools.MKVFile.OperationResult>(MKVTools.MKVFile.OperationResult.FileSaved, resultActual, string.Format("{0} ({1}): MKVFile.Save() not as expected after MKVPropEdit reset to original values.", desc[i], mkvFiles[j]));
                    }
                }
            }
        }
    }
}