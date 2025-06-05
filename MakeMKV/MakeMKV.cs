using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using Microsoft.Win32;
using System.Security.Cryptography;
using EventLogger;

namespace MKVTools
{
    public class MakeMKV
    {
        public MakeMKV()
        {
        }

        private readonly string registryDefaultSelectionStringKeyName = @"HKEY_CURRENT_USER\Software\MakeMKV";
        private readonly string registryDefaultSelectionStringValueName = "app_DefaultSelectionString";
        private string defaultSelectionString = null;

        private IEventLogger eventLogger = null;
        public IEventLogger EventLogger
        {
            get { return eventLogger; }
            set { eventLogger = value; }
        }

        public bool BackupDefaultSelectionString()
        {
            try
            { defaultSelectionString = Registry.GetValue(registryDefaultSelectionStringKeyName, registryDefaultSelectionStringValueName, "").ToString(); return true; }
            catch
            { return false; }
        }

        public bool RestoreDefaultSelectionString()
        {
            return (defaultSelectionString != null ? setDefaultSelectionString(defaultSelectionString) : false);
        }

        public bool ClearDefaultSelectionString()
        {
            return setDefaultSelectionString("");
        }

        public bool SetDefaultSelectionString(string Value)
        {
            return (Value != null ? setDefaultSelectionString(Value) : false);
        }

        private bool setDefaultSelectionString(string value)
        {
            try
            { Registry.SetValue(registryDefaultSelectionStringKeyName, registryDefaultSelectionStringValueName, value); return true; }
            catch
            { return false; }
        }

        public byte GetDriveIndex(string Drive)
        {
            // Execute: makemkvcon -r --cache=1 info disc:9999
            // Look for: DRV:<index>,0-999,0-999,0-999,"<text>","<text>","<location>"
            // If location matches, return index.

            // Verify and modify Drive so it matches MakeMKV format
            if (Drive == null || Drive.Length == 0) return 255;
            Drive = Drive.TrimEnd('\\').ToUpper();
            if (!Drive.EndsWith(":")) Drive += ":";

            string cmd;
            bool is64bit = Environment.Is64BitOperatingSystem;
            cmd = MakeMKVPath + "\\makemkvcon" + (is64bit ? "64" : "") + ".exe";
            if (!File.Exists(cmd))
                return 255;

            string args = "-r --cache=1 info disc:9999";

            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = args;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            if (!process.Start())
                return 255;

            string line;
            Regex regex = new Regex("^DRV:([0-9]+),[0-9]{1,3},[0-9]{1,3},[0-9]{1,3},\"[\\w-. ]*\",\"[\\w-. ]*\",\"([A-Z]:)\"$");
            Match match;
            while (!process.StandardOutput.EndOfStream)
            {
                line = process.StandardOutput.ReadLine();
                Console.WriteLine(line);
                match = regex.Match(line);
                if (match.Success)
                    if (match.Groups[2].Value == Drive) return byte.Parse(match.Groups[1].Value);
            }

            return 255;
        }

        #region Properties

        private object lockProperties = new object();

        private string makeMkvPath = @"C:\Program Files (x86)\MakeMKV";
        public string MakeMKVPath 
        {
            get { lock(lockProperties) return makeMkvPath; }
            set
            {
                if (value == null) return;

                string path = value.Trim();
                if (path.EndsWith("\\")) path = path.Substring(0, path.Length - 1);

                lock (lockProperties) makeMkvPath = path;
            }
        }

        public bool Available
        {
            get
            {
                lock (lockProperties)
                {
                    return File.Exists(this.MakeMKVPath + "\\makemkvcon" + (Environment.Is64BitOperatingSystem ? "64" : "") + ".exe");
                }
            }
        }

        private ushort minimumTitleLength = 300;
        public ushort MinimumTitleLength { get { lock (lockProperties) return minimumTitleLength; } set { lock (lockProperties) minimumTitleLength = (minimumTitleLength > (ushort)0) ? value : (ushort)0; } }

        private string defaultOutputFolder = null;
        public string DefaultOutputFolder { get { lock (lockProperties) return defaultOutputFolder; } set { lock (lockProperties) defaultOutputFolder = value; } }

        private bool allowOutputFileOverwrite;
        public bool AllowOutputFileOverwrite { get { lock (lockProperties) return allowOutputFileOverwrite; } set { lock (lockProperties) allowOutputFileOverwrite = value; } }

        private bool ignoreForcedSubtitlesFlag = true;
        public bool IgnoreForcedSubtitlesFlag { get { lock (lockProperties) return ignoreForcedSubtitlesFlag; } set { lock (lockProperties) { ignoreForcedSubtitlesFlag = value; conversionProfileCache = null; } } }

        private bool setFirstAudioTrackAsDefault = true;
        public bool SetFirstAudioTrackAsDefault { get { lock (lockProperties) return setFirstAudioTrackAsDefault; } set { lock (lockProperties) { setFirstAudioTrackAsDefault = value; conversionProfileCache = null; } } }

        private bool setFirstSubtitleTrackAsDefault = true;
        public bool SetFirstSubtitleTrackAsDefault { get { lock (lockProperties) return setFirstSubtitleTrackAsDefault; } set { lock (lockProperties) { setFirstSubtitleTrackAsDefault = value; conversionProfileCache = null; } } }

        private bool setFirstForcedSubtitleTrackAsDefault = true;
        public bool SetFirstForcedSubtitleTrackAsDefault { get { lock (lockProperties) return setFirstForcedSubtitleTrackAsDefault; } set { lock (lockProperties) { setFirstForcedSubtitleTrackAsDefault = value; conversionProfileCache = null; } } }

        private bool insertFirstChapterIfMissing = true;
        public bool InsertFirstChapterIfMissing { get { lock (lockProperties) return insertFirstChapterIfMissing; } set { lock (lockProperties) { insertFirstChapterIfMissing = value; conversionProfileCache = null; } } }

        // ModifyTrackSettingsAfterConversion delegate
        public delegate void ModifyTrackSettingsAfterConversionDelegate(List<Track> Tracks);
        private ModifyTrackSettingsAfterConversionDelegate modifyTrackSettingsAfterConversion;
        public ModifyTrackSettingsAfterConversionDelegate ModifyTrackSettingsAfterConversion
        {
            get { return modifyTrackSettingsAfterConversion; } 
            set { modifyTrackSettingsAfterConversion = value; } 
        }

        // Default audio output format for lossless tracks. Lossy tracks are by default always copied directly, unless explicitly given otherwise in AudioOutputFormatCustom.
        private AudioOutputFormat audioOutputFormatDefault = MKVTools.AudioOutputFormat.DirectCopy;
        public AudioOutputFormat AudioOutputFormatDefault { get { lock (lockProperties) return audioOutputFormatDefault; } set { lock (lockProperties) { audioOutputFormatDefault = value; conversionProfileCache = null; } } }

        private Dictionary<AudioCodec, AudioOutputFormat> audioOutputFormatCustom = new Dictionary<AudioCodec,AudioOutputFormat>();
        public Dictionary<AudioCodec, AudioOutputFormat> AudioOutputFormatCustom { get { lock (lockProperties) return audioOutputFormatCustom; } set { lock (lockProperties) { audioOutputFormatCustom = value; conversionProfileCache = null; } } }

        public static AudioCodec GetAudioCodecByIdentifier(string Identifier)
        {
            switch(Identifier)
            {
                case "LPCM-stereo": return AudioCodec.LPCM_Stereo;
                case "LPCM-multi": return AudioCodec.LPCM_Multi;
                case "FLAC-stereo": return AudioCodec.FLAC_Stereo;
                case "FLAC-multi": return AudioCodec.FLAC_Multi;
                case "MLP-stereo": return AudioCodec.MLP_Stereo;
                case "MLP-multi": return AudioCodec.MLP_Multi;
                case "DTSHDMA-stereo": return AudioCodec.DTSHDMasterAudio_Stereo;
                case "DTSHDMA-multi": return AudioCodec.DTSHDMasterAudio_Multi;
                case "TRUEHD-stereo": return AudioCodec.TrueHD_Stereo;
                case "TRUEHD-multi": return AudioCodec.TrueHD_Multi;
                case "MP2": return AudioCodec.MP2;
                case "MP3": return AudioCodec.MP3;
                case "AC3-stereo": return AudioCodec.AC3_Stereo;
                case "AC3-multi": return AudioCodec.AC3_Multi;
                case "DTS-stereo": return AudioCodec.DTS_Stereo;
                case "DTS-multi": return AudioCodec.DTS_Multi;
                case "EAC3-stereo": return AudioCodec.EAC3_Stereo;
                case "EAC3-multi": return AudioCodec.EAC3_Multi;
                case "DTSHD-stereo": return AudioCodec.DTSHD_Stereo;
                case "DTSHD-multi": return AudioCodec.DTSHD_Multi;
                case "DTSHDLBR-stereo": return AudioCodec.DTSHDLBR_Stereo;
                case "DTSHDLBR-multi": return AudioCodec.DTSHDLBR_Multi;
                case "DTSHD-core-stereo": return AudioCodec.DTSHD_Core_Stereo;
                case "DTSHD-core-multi": return AudioCodec.DTSHD_Core_Multi;
                case "TRUEHD-core-stereo": return AudioCodec.TrueHD_Core_Stereo;
                case "TRUEHD-core-multi": return AudioCodec.TrueHD_Core_Multi;
                default: return AudioCodec.Unknown;
            }
        }

        public static string GetAudioCodecIdentifier(AudioCodec Codec)
        {
            switch (Codec)
            {
                case AudioCodec.LPCM_Stereo: return "LPCM-stereo";
                case AudioCodec.LPCM_Multi: return "LPCM-multi";
                case AudioCodec.FLAC_Stereo: return "FLAC-stereo";
                case AudioCodec.FLAC_Multi: return "FLAC-multi";
                case AudioCodec.MLP_Stereo: return "MLP-stereo";
                case AudioCodec.MLP_Multi: return "MLP-multi";
                case AudioCodec.DTSHDMasterAudio_Stereo: return "DTSHDMA-stereo";
                case AudioCodec.DTSHDMasterAudio_Multi: return "DTSHDMA-multi";
                case AudioCodec.TrueHD_Stereo: return "TRUEHD-stereo";
                case AudioCodec.TrueHD_Multi: return "TRUEHD-multi";
                case AudioCodec.MP2: return "MP2";
                case AudioCodec.MP3: return "MP3";
                case AudioCodec.AC3_Stereo: return "AC3-stereo";
                case AudioCodec.AC3_Multi: return "AC3-multi";
                case AudioCodec.DTS_Stereo: return "DTS-stereo";
                case AudioCodec.DTS_Multi: return "DTS-multi";
                case AudioCodec.EAC3_Stereo: return "EAC3-stereo";
                case AudioCodec.EAC3_Multi: return "EAC3-multi";
                case AudioCodec.DTSHD_Stereo: return "DTSHD-stereo";
                case AudioCodec.DTSHD_Multi: return "DTSHD-multi";
                case AudioCodec.DTSHDLBR_Stereo: return "DTSHDLBR-stereo";
                case AudioCodec.DTSHDLBR_Multi: return "DTSHDLBR-multi";
                case AudioCodec.DTSHD_Core_Stereo: return "DTSHD-core-stereo";
                case AudioCodec.DTSHD_Core_Multi: return "DTSHD-core-multi";
                case AudioCodec.TrueHD_Core_Stereo: return "TRUEHD-core-stereo";
                case AudioCodec.TrueHD_Core_Multi: return "TRUEHD-core-multi";
                default: return null;
            }
        }

        public static bool GetAudioCodecIsLossless(AudioCodec Codec)
        {
            switch (Codec)
            {
                case AudioCodec.LPCM_Stereo:
                case AudioCodec.LPCM_Multi:
                case AudioCodec.FLAC_Stereo:
                case AudioCodec.FLAC_Multi:
                case AudioCodec.MLP_Stereo:
                case AudioCodec.MLP_Multi:
                case AudioCodec.DTSHDMasterAudio_Stereo:
                case AudioCodec.DTSHDMasterAudio_Multi:
                case AudioCodec.TrueHD_Stereo:
                case AudioCodec.TrueHD_Multi:
                    return true;
                default: 
                    return false;
            }
        }

        public AudioOutputFormat AudioOutputFormat(AudioCodec Codec)
        {
            lock (lockProperties)
            {
                if (audioOutputFormatCustom.ContainsKey(Codec))
                { return audioOutputFormatCustom[Codec]; }
                else
                { return audioOutputFormatDefault; }
            }
        }

        private SubtitleCompression subtitleCompression = SubtitleCompression.zlib;
        public SubtitleCompression SubtitleCompression { get { lock (lockProperties) return subtitleCompression; } set { lock (lockProperties) { subtitleCompression = value; conversionProfileCache = null; } } }

        private FLACCompression flacCompression = FLACCompression.Fast;
        public FLACCompression FLACCompression { get { lock (lockProperties) return flacCompression; } set { lock (lockProperties) { flacCompression = value; conversionProfileCache = null; } } }

        private LPCMContainer lpcmContainer = LPCMContainer.Raw;
        public LPCMContainer LPCMContainer { get { lock (lockProperties) return lpcmContainer; } set { lock (lockProperties) { lpcmContainer = value; conversionProfileCache = null; } } }

        #endregion

        #region Conversion profile

        public bool CreateConversionProfile(string Filename, string TrackSelection)
        {
            try { File.WriteAllText(Filename, getConversionProfile(TrackSelection)); }
            catch { return false; }

            return true;
        }


        private enum TrackSettingsSelectionRule
        {
            None = 0,
            Default = 1,
            Explicit = 2
        }

        private readonly TrackSettingsSelectionRule specifySelectionString = TrackSettingsSelectionRule.Default;
        private string conversionProfileCache = null;
        private string getConversionProfile(string selection)
        {
            if (conversionProfileCache == null)
            {
                StringBuilder xml = new StringBuilder();
                xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?><profile><name lang=\"eng\">Custom</name>");

                // Common settings/flags
                xml.AppendLine(String.Format("<mkvSettings useISO639Type2T=\"true\" ignoreForcedSubtitlesFlag=\"{0}\" setFirstAudioTrackAsDefault=\"{1}\" setFirstSubtitleTrackAsDefault=\"{2}\" setFirstForcedSubtitleTrackAsDefault=\"{3}\" insertFirstChapter00IfMissing=\"{4}\" />",
                    this.IgnoreForcedSubtitlesFlag.ToString().ToLower(),
                    this.SetFirstAudioTrackAsDefault.ToString().ToLower(),
                    this.SetFirstSubtitleTrackAsDefault.ToString().ToLower(),
                    this.SetFirstForcedSubtitleTrackAsDefault.ToString().ToLower(),
                    this.InsertFirstChapterIfMissing.ToString().ToLower()
                    ));

                // Selection string
                xml.AppendLine("<profileSettings app_DefaultSelectionString=\"{selection}\" />");

                // Output formats
                xml.AppendLine("<outputSettings name=\"DirectCopy\" outputFormat=\"directCopy\"><description lang=\"eng\">DirectCopy</description></outputSettings>");

                switch (this.LPCMContainer)
                {
                    case LPCMContainer.Raw:
                        xml.AppendLine("<outputSettings name=\"LPCM\" outputFormat=\"LPCM-raw\"><description lang=\"eng\">LPCM-Raw</description></outputSettings>");
                        break;
                    case LPCMContainer.Wavex:
                        xml.AppendLine("<outputSettings name=\"LPCM\" outputFormat=\"LPCM-wavex\"><description lang=\"eng\">LPCM-Wavex</description></outputSettings>");
                        break;
                }

                int cores = (int)Math.Floor((double)Environment.ProcessorCount / 2);
                if (cores < 1) cores = 1;
                switch (this.FLACCompression)
                {
                    case FLACCompression.Fast:
                        xml.AppendLine("<outputSettings name=\"FLAC\" outputFormat=\"FLAC\"><description lang=\"eng\">FLAC-Fast</description><extraArgs>-threads " + cores + " -compression_level 5</extraArgs></outputSettings>");
                        break;
                    case FLACCompression.Good:
                        xml.AppendLine("<outputSettings name=\"FLAC\" outputFormat=\"FLAC\"><description lang=\"eng\">FLAC-Good</description><extraArgs>-threads " + cores + " -compression_level 12</extraArgs></outputSettings>");
                        break;
                    case FLACCompression.Best:
                        xml.AppendLine("<outputSettings name=\"FLAC\" outputFormat=\"FLAC\"><description lang=\"eng\">FLAC-Best</description><extraArgs>-threads " + cores + " -compression_level 12 -lpc_coeff_precision 15 -lpc_passes 8 -lpc_type 3</extraArgs></outputSettings>");
                        break;
                }

                switch (this.SubtitleCompression)
                {
                    case SubtitleCompression.zlib:
                        xml.AppendLine("<outputSettings name=\"zlib\" outputFormat=\"directCopy\"><description lang=\"eng\">zlib</description><extraArgs>compression=\"zlib\" compressionLevel=\"9\"</extraArgs></outputSettings>");
                        break;
                    default:
                        xml.AppendLine("<outputSettings name=\"subs\" outputFormat=\"directCopy\"><description lang=\"eng\">subs</description></outputSettings>");
                        break;
                }

                string trackDefaultSelection;
                switch (specifySelectionString)
                {
                    case TrackSettingsSelectionRule.Default: trackDefaultSelection = " defaultSelection=\"$app_DefaultSelectionString\""; break;
                    case TrackSettingsSelectionRule.Explicit: trackDefaultSelection = String.Format(" defaultSelection=\"{0}\"", selection); break;
                    default: trackDefaultSelection = ""; break;
                }

                // Track settings - conversion/compression
                var codecs = Enum.GetValues(typeof(AudioCodec));
                string audioCodecIdentifier;
                AudioOutputFormat audioOutputFormat;
                foreach (AudioCodec codec in codecs)
                {
                    if (codec == AudioCodec.Unknown) continue;
                    audioOutputFormat = this.AudioOutputFormat(codec);
                    audioCodecIdentifier = GetAudioCodecIdentifier(codec);
                    xml.AppendLine(String.Format("<trackSettings input=\"{0}\"><output outputSettingsName=\"{1}\"{2} /></trackSettings>",
                        audioCodecIdentifier,
                        audioOutputFormat.ToString(),
                        trackDefaultSelection));
                }

                string[] subtitles = new string[6] { "PGS", "PGS-forced", "VOBSUB", "VOBSUB-forced", "VOBSUBHD", "VOBSUBHD-forced" };
                foreach (string subtitle in subtitles)
                {
                    xml.AppendLine(String.Format("<trackSettings input=\"{0}\"><output outputSettingsName=\"{1}\"{2} /></trackSettings>",
                           subtitle,
                           this.SubtitleCompression == MKVTools.SubtitleCompression.zlib ? "zlib" : "subs",
                           trackDefaultSelection));
                }

                xml.Append("</profile>");
                conversionProfileCache = xml.ToString();
            }

            return conversionProfileCache.Replace("{selection}", selection);
        }

        #endregion

        #region Static enum functions (enum <-> identifier)

        public static Language GetLanguage(string ISOCode)
        {
            switch(ISOCode.Trim().ToLower())
            {
                case "abk": return Language.Abkhazian;
                case "ace": return Language.Achinese;
                case "ach": return Language.Acoli;
                case "ada": return Language.Adangme;
                case "ady": return Language.Adyghe;
                case "aar": return Language.Afar;
                case "afh": return Language.Afrihili;
                case "afr": return Language.Afrikaans;
                case "ain": return Language.Ainu;
                case "aka": return Language.Akan;
                case "sqi": case "alb": return Language.Albanian;
                case "ale": return Language.Aleut;
                case "amh": return Language.Amharic;
                case "anp": return Language.Angika;
                case "ara": return Language.Arabic;
                case "arg": return Language.Aragonese;
                case "arp": return Language.Arapaho;
                case "arw": return Language.Arawak;
                case "hye": case "arm": return Language.Armenian;
                case "asm": return Language.Assamese;
                case "ast": return Language.Asturian;
                case "ava": return Language.Avaric;
                case "awa": return Language.Awadhi;
                case "aym": return Language.Aymara;
                case "aze": return Language.Azerbaijani;
                case "ban": return Language.Balinese;
                case "bal": return Language.Baluchi;
                case "bam": return Language.Bambara;
                case "bas": return Language.Basa;
                case "bak": return Language.Bashkir;
                case "eus": case "baq": return Language.Basque;
                case "bej": return Language.Beja;
                case "bel": return Language.Belarusian;
                case "bem": return Language.Bemba;
                case "ben": return Language.Bengali;
                case "bho": return Language.Bhojpuri;
                case "bik": return Language.Bikol;
                case "byn": return Language.Bilin;
                case "bin": return Language.Bini;
                case "bis": return Language.Bislama;
                case "bos": return Language.Bosnian;
                case "bra": return Language.Braj;
                case "bre": return Language.Breton;
                case "bug": return Language.Buginese;
                case "bul": return Language.Bulgarian;
                case "bua": return Language.Buriat;
                case "mya": case "bur": return Language.Burmese;
                case "cad": return Language.Caddo;
                case "cat": return Language.Catalan;
                case "ceb": return Language.Cebuano;
                case "khm": return Language.CentralKhmer;
                case "cha": return Language.Chamorro;
                case "che": return Language.Chechen;
                case "chr": return Language.Cherokee;
                case "chy": return Language.Cheyenne;
                case "zho": case "chi": return Language.Chinese;
                case "chn": return Language.Chinookjargon;
                case "chp": return Language.Chipewyan;
                case "cho": return Language.Choctaw;
                case "chk": return Language.Chuukese;
                case "chv": return Language.Chuvash;
                case "cor": return Language.Cornish;
                case "cos": return Language.Corsican;
                case "cre": return Language.Cree;
                case "mus": return Language.Creek;
                case "crh": return Language.CrimeanTatar;
                case "hrv": return Language.Croatian;
                case "ces": case "cze": return Language.Czech;
                case "dak": return Language.Dakota;
                case "dan": return Language.Danish;
                case "dar": return Language.Dargwa;
                case "del": return Language.Delaware;
                case "div": return Language.Dhivehi;
                case "din": return Language.Dinka;
                case "doi": return Language.Dogri;
                case "dgr": return Language.Dogrib;
                case "dua": return Language.Duala;
                case "nld": case "dut": return Language.Dutch;
                case "dyu": return Language.Dyula;
                case "dzo": return Language.Dzongkha;
                case "frs": return Language.EasternFrisian;
                case "efi": return Language.Efik;
                case "eka": return Language.Ekajuk;
                case "eng": return Language.English;
                case "myv": return Language.Erzya;
                case "epo": return Language.Esperanto;
                case "est": return Language.Estonian;
                case "ewe": return Language.Ewe;
                case "ewo": return Language.Ewondo;
                case "fan": return Language.Fang;
                case "fat": return Language.Fanti;
                case "fao": return Language.Faroese;
                case "fij": return Language.Fijian;
                case "fil": return Language.Filipino;
                case "fin": return Language.Finnish;
                case "fon": return Language.Fon;
                case "fra": case "fre": return Language.French;
                case "fur": return Language.Friulian;
                case "ful": return Language.Fulah;
                case "gaa": return Language.Ga;
                case "car": return Language.GalibiCarib;
                case "glg": return Language.Galician;
                case "lug": return Language.Ganda;
                case "gay": return Language.Gayo;
                case "gba": return Language.Gbaya;
                case "kat": case "geo": return Language.Georgian;
                case "deu": case "ger": return Language.German;
                case "gil": return Language.Gilbertese;
                case "gon": return Language.Gondi;
                case "gor": return Language.Gorontalo;
                case "grb": return Language.Grebo;
                case "grn": return Language.Guarani;
                case "guj": return Language.Gujarati;
                case "gwi": return Language.Gwichin;
                case "hai": return Language.Haida;
                case "hat": return Language.Haitian;
                case "hau": return Language.Hausa;
                case "haw": return Language.Hawaiian;
                case "heb": return Language.Hebrew;
                case "her": return Language.Herero;
                case "hil": return Language.Hiligaynon;
                case "hin": return Language.Hindi;
                case "hmo": return Language.HiriMotu;
                case "hmn": return Language.Hmong;
                case "hun": return Language.Hungarian;
                case "hup": return Language.Hupa;
                case "iba": return Language.Iban;
                case "isl": case "ice": return Language.Icelandic;
                case "ido": return Language.Ido;
                case "ibo": return Language.Igbo;
                case "ilo": return Language.Iloko;
                case "smn": return Language.InariSami;
                case "ind": return Language.Indonesian;
                case "inh": return Language.Ingush;
                case "iku": return Language.Inuktitut;
                case "ipk": return Language.Inupiaq;
                case "gle": return Language.Irish;
                case "ita": return Language.Italian;
                case "jpn": return Language.Japanese;
                case "jav": return Language.Javanese;
                case "jrb": return Language.JudeoArabic;
                case "jpr": return Language.JudeoPersian;
                case "kbd": return Language.Kabardian;
                case "kab": return Language.Kabyle;
                case "kac": return Language.Kachin;
                case "xal": return Language.Kalmyk;
                case "kal": return Language.Kalaallisut;
                case "kam": return Language.Kamba;
                case "kan": return Language.Kannada;
                case "kau": return Language.Kanuri;
                case "krc": return Language.KarachayBalkar;
                case "kaa": return Language.KaraKalpak;
                case "krl": return Language.Karelian;
                case "kas": return Language.Kashmiri;
                case "csb": return Language.Kashubian;
                case "kaz": return Language.Kazakh;
                case "kha": return Language.Khasi;
                case "kik": return Language.Kikuyu;
                case "kmb": return Language.Kimbundu;
                case "kin": return Language.Kinyarwanda;
                case "kir": return Language.Kirghiz;
                case "tlh": return Language.Klingon;
                case "kom": return Language.Komi;
                case "kon": return Language.Kongo;
                case "kok": return Language.Konkani;
                case "kor": return Language.Korean;
                case "kos": return Language.Kosraean;
                case "kpe": return Language.Kpelle;
                case "kua": return Language.Kuanyama;
                case "kum": return Language.Kumyk;
                case "kur": return Language.Kurdish;
                case "kru": return Language.Kurukh;
                case "kut": return Language.Kutenai;
                case "lad": return Language.Ladino;
                case "lah": return Language.Lahnda;
                case "lam": return Language.Lamba;
                case "lao": return Language.Lao;
                case "lat": return Language.Latin;
                case "lav": return Language.Latvian;
                case "lez": return Language.Lezghian;
                case "lim": return Language.Limburgan;
                case "lin": return Language.Lingala;
                case "lit": return Language.Lithuanian;
                case "jbo": return Language.Lojban;
                case "dsb": return Language.LowerSorbian;
                case "nds": return Language.LowGerman;
                case "loz": return Language.Lozi;
                case "lub": return Language.LubaKatanga;
                case "lua": return Language.LubaLulua;
                case "lui": return Language.Luiseno;
                case "smj": return Language.LuleSami;
                case "lun": return Language.Lunda;
                case "luo": return Language.Luo;
                case "lus": return Language.Lushai;
                case "ltz": return Language.Luxembourgish;
                case "mkd": case "mac": return Language.Macedonian;
                case "rup": return Language.MacedoRomanian;
                case "mad": return Language.Madurese;
                case "mag": return Language.Magahi;
                case "mai": return Language.Maithili;
                case "mak": return Language.Makasar;
                case "mlg": return Language.Malagasy;
                case "msa": case "may": return Language.Malay;
                case "mal": return Language.Malayalam;
                case "mlt": return Language.Maltese;
                case "mnc": return Language.Manchu;
                case "mdr": return Language.Mandar;
                case "man": return Language.Mandingo;
                case "mni": return Language.Manipuri;
                case "glv": return Language.Manx;
                case "mri": case "mao": return Language.Maori;
                case "arn": return Language.Mapudungun;
                case "mar": return Language.Marathi;
                case "chm": return Language.Mari;
                case "mah": return Language.Marshallese;
                case "mwr": return Language.Marwari;
                case "mas": return Language.Masai;
                case "men": return Language.Mende;
                case "mic": return Language.Mikmaq;
                case "min": return Language.Minangkabau;
                case "mwl": return Language.Mirandese;
                case "ell": case "gre": return Language.Greek;
                case "moh": return Language.Mohawk;
                case "mdf": return Language.Moksha;
                case "lol": return Language.Mongo;
                case "mon": return Language.Mongolian;
                case "mos": return Language.Mossi;
                case "mul": return Language.Multiplelanguages;
                case "nau": return Language.Nauru;
                case "nav": return Language.Navajo;
                case "ndo": return Language.Ndonga;
                case "nap": return Language.Neapolitan;
                case "new": return Language.NepalBhasa;
                case "nep": return Language.Nepali;
                case "nia": return Language.Nias;
                case "niu": return Language.Niuean;
                case "nqo": return Language.NKo;
                case "nog": return Language.Nogai;
                case "zxx": return Language.Nolinguisticcontent;
                case "frr": return Language.NorthernFrisian;
                case "sme": return Language.NorthernSami;
                case "nde": return Language.NorthNdebele;
                case "nor": return Language.Norwegian;
                case "nob": return Language.NorwegianBokmål;
                case "nno": return Language.NorwegianNynorsk;
                case "nym": return Language.Nyamwezi;
                case "nya": return Language.Nyanja;
                case "nyn": return Language.Nyankole;
                case "nyo": return Language.Nyoro;
                case "nzi": return Language.Nzima;
                case "oci": return Language.Occitan;
                case "ori": return Language.Odiya;
                case "oji": return Language.Ojibwa;
                case "orm": return Language.Oromo;
                case "osa": return Language.Osage;
                case "oss": return Language.Ossetian;
                case "pal": return Language.Pahlavi;
                case "pau": return Language.Palauan;
                case "pam": return Language.Pampanga;
                case "pag": return Language.Pangasinan;
                case "pan": return Language.Panjabi;
                case "pap": return Language.Papiamento;
                case "nso": return Language.Pedi;
                case "fas": case "per": return Language.Persian;
                case "pon": return Language.Pohnpeian;
                case "pol": return Language.Polish;
                case "por": return Language.Portuguese;
                case "pus": return Language.Pushto;
                case "que": return Language.Quechua;
                case "raj": return Language.Rajasthani;
                case "rap": return Language.Rapanui;
                case "rar": return Language.Rarotongan;
                case "ron": case "rum": return Language.Romanian;
                case "roh": return Language.Romansh;
                case "rom": return Language.Romany;
                case "run": return Language.Rundi;
                case "rus": return Language.Russian;
                case "smo": return Language.Samoan;
                case "sad": return Language.Sandawe;
                case "sag": return Language.Sango;
                case "san": return Language.Sanskrit;
                case "sat": return Language.Santali;
                case "srd": return Language.Sardinian;
                case "sas": return Language.Sasak;
                case "sco": return Language.Scots;
                case "gla": return Language.ScottishGaelic;
                case "sel": return Language.Selkup;
                case "srp": return Language.Serbian;
                case "srr": return Language.Serer;
                case "shn": return Language.Shan;
                case "sna": return Language.Shona;
                case "iii": return Language.SichuanYi;
                case "scn": return Language.Sicilian;
                case "sid": return Language.Sidamo;
                case "bla": return Language.Siksika;
                case "snd": return Language.Sindhi;
                case "sin": return Language.Sinhala;
                case "sms": return Language.SkoltSami;
                case "den": return Language.Slave;
                case "slk": case "slo": return Language.Slovak;
                case "slv": return Language.Slovenian;
                case "sog": return Language.Sogdian;
                case "som": return Language.Somali;
                case "snk": return Language.Soninke;
                case "alt": return Language.SouthernAltai;
                case "sma": return Language.SouthernSami;
                case "sot": return Language.SouthernSotho;
                case "nbl": return Language.SouthNdebele;
                case "spa": return Language.Spanish;
                case "srn": return Language.SrananTongo;
                case "zgh": return Language.StandardMoroccanTamazight;
                case "suk": return Language.Sukuma;
                case "sux": return Language.Sumerian;
                case "sun": return Language.Sundanese;
                case "sus": return Language.Susu;
                case "swa": return Language.Swahili;
                case "ssw": return Language.Swati;
                case "swe": return Language.Swedish;
                case "gsw": return Language.SwissGerman;
                case "syr": return Language.Syriac;
                case "tgl": return Language.Tagalog;
                case "tah": return Language.Tahitian;
                case "tgk": return Language.Tajik;
                case "tmh": return Language.Tamashek;
                case "tam": return Language.Tamil;
                case "tat": return Language.Tatar;
                case "tel": return Language.Telugu;
                case "ter": return Language.Tereno;
                case "tet": return Language.Tetum;
                case "tha": return Language.Thai;
                case "bod": case "tib": return Language.Tibetan;
                case "tig": return Language.Tigre;
                case "tir": return Language.Tigrinya;
                case "tem": return Language.Timne;
                case "tiv": return Language.Tiv;
                case "tli": return Language.Tlingit;
                case "tkl": return Language.Tokelau;
                case "tpi": return Language.TokPisin;
                case "tog": return Language.TongaNyasa;
                case "ton": return Language.TongaTongaIslands;
                case "tsi": return Language.Tsimshian;
                case "tso": return Language.Tsonga;
                case "tsn": return Language.Tswana;
                case "tum": return Language.Tumbuka;
                case "tur": return Language.Turkish;
                case "tuk": return Language.Turkmen;
                case "tvl": return Language.Tuvalu;
                case "tyv": return Language.Tuvinian;
                case "twi": return Language.Twi;
                case "udm": return Language.Udmurt;
                case "uga": return Language.Ugaritic;
                case "uig": return Language.Uighur;
                case "ukr": return Language.Ukrainian;
                case "umb": return Language.Umbundu;
                case "mis": return Language.Uncodedlanguages;
                case "hsb": return Language.UpperSorbian;
                case "urd": return Language.Urdu;
                case "uzb": return Language.Uzbek;
                case "vai": return Language.Vai;
                case "ven": return Language.Venda;
                case "vie": return Language.Vietnamese;
                case "vot": return Language.Votic;
                case "wln": return Language.Walloon;
                case "war": return Language.Waray;
                case "was": return Language.Washo;
                case "cym": case "wel": return Language.Welsh;
                case "fry": return Language.WesternFrisian;
                case "wal": return Language.Wolaytta;
                case "wol": return Language.Wolof;
                case "xho": return Language.Xhosa;
                case "sah": return Language.Yakut;
                case "yao": return Language.Yao;
                case "yap": return Language.Yapese;
                case "yid": return Language.Yiddish;
                case "yor": return Language.Yoruba;
                case "zap": return Language.Zapotec;
                case "zza": return Language.Zaza;
                case "zen": return Language.Zenaga;
                case "zha": return Language.Zhuang;
                case "zul": return Language.Zulu;
                case "zun": return Language.Zuni;
                default: return Language.Undetermined;
            }
        }

        public static string GetLanguageISOCode(Language Language)
        {
            switch(Language)
            {
                case Language.Abkhazian: return "abk";
                case Language.Achinese: return "ace";
                case Language.Acoli: return "ach";
                case Language.Adangme: return "ada";
                case Language.Adyghe: return "ady";
                case Language.Afar: return "aar";
                case Language.Afrihili: return "afh";
                case Language.Afrikaans: return "afr";
                case Language.Ainu: return "ain";
                case Language.Akan: return "aka";
                case Language.Albanian: return "sqi";
                case Language.Aleut: return "ale";
                case Language.Amharic: return "amh";
                case Language.Angika: return "anp";
                case Language.Arabic: return "ara";
                case Language.Aragonese: return "arg";
                case Language.Arapaho: return "arp";
                case Language.Arawak: return "arw";
                case Language.Armenian: return "hye";
                case Language.Assamese: return "asm";
                case Language.Asturian: return "ast";
                case Language.Avaric: return "ava";
                case Language.Awadhi: return "awa";
                case Language.Aymara: return "aym";
                case Language.Azerbaijani: return "aze";
                case Language.Balinese: return "ban";
                case Language.Baluchi: return "bal";
                case Language.Bambara: return "bam";
                case Language.Basa: return "bas";
                case Language.Bashkir: return "bak";
                case Language.Basque: return "eus";
                case Language.Beja: return "bej";
                case Language.Belarusian: return "bel";
                case Language.Bemba: return "bem";
                case Language.Bengali: return "ben";
                case Language.Bhojpuri: return "bho";
                case Language.Bikol: return "bik";
                case Language.Bilin: return "byn";
                case Language.Bini: return "bin";
                case Language.Bislama: return "bis";
                case Language.Bosnian: return "bos";
                case Language.Braj: return "bra";
                case Language.Breton: return "bre";
                case Language.Buginese: return "bug";
                case Language.Bulgarian: return "bul";
                case Language.Buriat: return "bua";
                case Language.Burmese: return "mya";
                case Language.Caddo: return "cad";
                case Language.Catalan: return "cat";
                case Language.Cebuano: return "ceb";
                case Language.CentralKhmer: return "khm";
                case Language.Chamorro: return "cha";
                case Language.Chechen: return "che";
                case Language.Cherokee: return "chr";
                case Language.Cheyenne: return "chy";
                case Language.Chinese: return "zho";
                case Language.Chinookjargon: return "chn";
                case Language.Chipewyan: return "chp";
                case Language.Choctaw: return "cho";
                case Language.Chuukese: return "chk";
                case Language.Chuvash: return "chv";
                case Language.Cornish: return "cor";
                case Language.Corsican: return "cos";
                case Language.Cree: return "cre";
                case Language.Creek: return "mus";
                case Language.CrimeanTatar: return "crh";
                case Language.Croatian: return "hrv";
                case Language.Czech: return "ces";
                case Language.Dakota: return "dak";
                case Language.Danish: return "dan";
                case Language.Dargwa: return "dar";
                case Language.Delaware: return "del";
                case Language.Dhivehi: return "div";
                case Language.Dinka: return "din";
                case Language.Dogri: return "doi";
                case Language.Dogrib: return "dgr";
                case Language.Duala: return "dua";
                case Language.Dutch: return "nld";
                case Language.Dyula: return "dyu";
                case Language.Dzongkha: return "dzo";
                case Language.EasternFrisian: return "frs";
                case Language.Efik: return "efi";
                case Language.Ekajuk: return "eka";
                case Language.English: return "eng";
                case Language.Erzya: return "myv";
                case Language.Esperanto: return "epo";
                case Language.Estonian: return "est";
                case Language.Ewe: return "ewe";
                case Language.Ewondo: return "ewo";
                case Language.Fang: return "fan";
                case Language.Fanti: return "fat";
                case Language.Faroese: return "fao";
                case Language.Fijian: return "fij";
                case Language.Filipino: return "fil";
                case Language.Finnish: return "fin";
                case Language.Fon: return "fon";
                case Language.French: return "fra";
                case Language.Friulian: return "fur";
                case Language.Fulah: return "ful";
                case Language.Ga: return "gaa";
                case Language.GalibiCarib: return "car";
                case Language.Galician: return "glg";
                case Language.Ganda: return "lug";
                case Language.Gayo: return "gay";
                case Language.Gbaya: return "gba";
                case Language.Georgian: return "kat";
                case Language.German: return "deu";
                case Language.Gilbertese: return "gil";
                case Language.Gondi: return "gon";
                case Language.Gorontalo: return "gor";
                case Language.Grebo: return "grb";
                case Language.Guarani: return "grn";
                case Language.Gujarati: return "guj";
                case Language.Gwichin: return "gwi";
                case Language.Haida: return "hai";
                case Language.Haitian: return "hat";
                case Language.Hausa: return "hau";
                case Language.Hawaiian: return "haw";
                case Language.Hebrew: return "heb";
                case Language.Herero: return "her";
                case Language.Hiligaynon: return "hil";
                case Language.Hindi: return "hin";
                case Language.HiriMotu: return "hmo";
                case Language.Hmong: return "hmn";
                case Language.Hungarian: return "hun";
                case Language.Hupa: return "hup";
                case Language.Iban: return "iba";
                case Language.Icelandic: return "isl";
                case Language.Ido: return "ido";
                case Language.Igbo: return "ibo";
                case Language.Iloko: return "ilo";
                case Language.InariSami: return "smn";
                case Language.Indonesian: return "ind";
                case Language.Ingush: return "inh";
                case Language.Inuktitut: return "iku";
                case Language.Inupiaq: return "ipk";
                case Language.Irish: return "gle";
                case Language.Italian: return "ita";
                case Language.Japanese: return "jpn";
                case Language.Javanese: return "jav";
                case Language.JudeoArabic: return "jrb";
                case Language.JudeoPersian: return "jpr";
                case Language.Kabardian: return "kbd";
                case Language.Kabyle: return "kab";
                case Language.Kachin: return "kac";
                case Language.Kalmyk: return "xal";
                case Language.Kalaallisut: return "kal";
                case Language.Kamba: return "kam";
                case Language.Kannada: return "kan";
                case Language.Kanuri: return "kau";
                case Language.KarachayBalkar: return "krc";
                case Language.KaraKalpak: return "kaa";
                case Language.Karelian: return "krl";
                case Language.Kashmiri: return "kas";
                case Language.Kashubian: return "csb";
                case Language.Kazakh: return "kaz";
                case Language.Khasi: return "kha";
                case Language.Kikuyu: return "kik";
                case Language.Kimbundu: return "kmb";
                case Language.Kinyarwanda: return "kin";
                case Language.Kirghiz: return "kir";
                case Language.Klingon: return "tlh";
                case Language.Komi: return "kom";
                case Language.Kongo: return "kon";
                case Language.Konkani: return "kok";
                case Language.Korean: return "kor";
                case Language.Kosraean: return "kos";
                case Language.Kpelle: return "kpe";
                case Language.Kuanyama: return "kua";
                case Language.Kumyk: return "kum";
                case Language.Kurdish: return "kur";
                case Language.Kurukh: return "kru";
                case Language.Kutenai: return "kut";
                case Language.Ladino: return "lad";
                case Language.Lahnda: return "lah";
                case Language.Lamba: return "lam";
                case Language.Lao: return "lao";
                case Language.Latin: return "lat";
                case Language.Latvian: return "lav";
                case Language.Lezghian: return "lez";
                case Language.Limburgan: return "lim";
                case Language.Lingala: return "lin";
                case Language.Lithuanian: return "lit";
                case Language.Lojban: return "jbo";
                case Language.LowerSorbian: return "dsb";
                case Language.LowGerman: return "nds";
                case Language.Lozi: return "loz";
                case Language.LubaKatanga: return "lub";
                case Language.LubaLulua: return "lua";
                case Language.Luiseno: return "lui";
                case Language.LuleSami: return "smj";
                case Language.Lunda: return "lun";
                case Language.Luo: return "luo";
                case Language.Lushai: return "lus";
                case Language.Luxembourgish: return "ltz";
                case Language.Macedonian: return "mkd";
                case Language.MacedoRomanian: return "rup";
                case Language.Madurese: return "mad";
                case Language.Magahi: return "mag";
                case Language.Maithili: return "mai";
                case Language.Makasar: return "mak";
                case Language.Malagasy: return "mlg";
                case Language.Malay: return "msa";
                case Language.Malayalam: return "mal";
                case Language.Maltese: return "mlt";
                case Language.Manchu: return "mnc";
                case Language.Mandar: return "mdr";
                case Language.Mandingo: return "man";
                case Language.Manipuri: return "mni";
                case Language.Manx: return "glv";
                case Language.Maori: return "mri";
                case Language.Mapudungun: return "arn";
                case Language.Marathi: return "mar";
                case Language.Mari: return "chm";
                case Language.Marshallese: return "mah";
                case Language.Marwari: return "mwr";
                case Language.Masai: return "mas";
                case Language.Mende: return "men";
                case Language.Mikmaq: return "mic";
                case Language.Minangkabau: return "min";
                case Language.Mirandese: return "mwl";
                case Language.Greek: return "ell";
                case Language.Mohawk: return "moh";
                case Language.Moksha: return "mdf";
                case Language.Mongo: return "lol";
                case Language.Mongolian: return "mon";
                case Language.Mossi: return "mos";
                case Language.Multiplelanguages: return "mul";
                case Language.Nauru: return "nau";
                case Language.Navajo: return "nav";
                case Language.Ndonga: return "ndo";
                case Language.Neapolitan: return "nap";
                case Language.NepalBhasa: return "new";
                case Language.Nepali: return "nep";
                case Language.Nias: return "nia";
                case Language.Niuean: return "niu";
                case Language.NKo: return "nqo";
                case Language.Nogai: return "nog";
                case Language.Nolinguisticcontent: return "zxx";
                case Language.NorthernFrisian: return "frr";
                case Language.NorthernSami: return "sme";
                case Language.NorthNdebele: return "nde";
                case Language.Norwegian: return "nor";
                case Language.NorwegianBokmål: return "nob";
                case Language.NorwegianNynorsk: return "nno";
                case Language.Nyamwezi: return "nym";
                case Language.Nyanja: return "nya";
                case Language.Nyankole: return "nyn";
                case Language.Nyoro: return "nyo";
                case Language.Nzima: return "nzi";
                case Language.Occitan: return "oci";
                case Language.Odiya: return "ori";
                case Language.Ojibwa: return "oji";
                case Language.Oromo: return "orm";
                case Language.Osage: return "osa";
                case Language.Ossetian: return "oss";
                case Language.Pahlavi: return "pal";
                case Language.Palauan: return "pau";
                case Language.Pampanga: return "pam";
                case Language.Pangasinan: return "pag";
                case Language.Panjabi: return "pan";
                case Language.Papiamento: return "pap";
                case Language.Pedi: return "nso";
                case Language.Persian: return "fas";
                case Language.Pohnpeian: return "pon";
                case Language.Polish: return "pol";
                case Language.Portuguese: return "por";
                case Language.Pushto: return "pus";
                case Language.Quechua: return "que";
                case Language.Rajasthani: return "raj";
                case Language.Rapanui: return "rap";
                case Language.Rarotongan: return "rar";
                case Language.Romanian: return "ron";
                case Language.Romansh: return "roh";
                case Language.Romany: return "rom";
                case Language.Rundi: return "run";
                case Language.Russian: return "rus";
                case Language.Samoan: return "smo";
                case Language.Sandawe: return "sad";
                case Language.Sango: return "sag";
                case Language.Sanskrit: return "san";
                case Language.Santali: return "sat";
                case Language.Sardinian: return "srd";
                case Language.Sasak: return "sas";
                case Language.Scots: return "sco";
                case Language.ScottishGaelic: return "gla";
                case Language.Selkup: return "sel";
                case Language.Serbian: return "srp";
                case Language.Serer: return "srr";
                case Language.Shan: return "shn";
                case Language.Shona: return "sna";
                case Language.SichuanYi: return "iii";
                case Language.Sicilian: return "scn";
                case Language.Sidamo: return "sid";
                case Language.Siksika: return "bla";
                case Language.Sindhi: return "snd";
                case Language.Sinhala: return "sin";
                case Language.SkoltSami: return "sms";
                case Language.Slave: return "den";
                case Language.Slovak: return "slk";
                case Language.Slovenian: return "slv";
                case Language.Sogdian: return "sog";
                case Language.Somali: return "som";
                case Language.Soninke: return "snk";
                case Language.SouthernAltai: return "alt";
                case Language.SouthernSami: return "sma";
                case Language.SouthernSotho: return "sot";
                case Language.SouthNdebele: return "nbl";
                case Language.Spanish: return "spa";
                case Language.SrananTongo: return "srn";
                case Language.StandardMoroccanTamazight: return "zgh";
                case Language.Sukuma: return "suk";
                case Language.Sumerian: return "sux";
                case Language.Sundanese: return "sun";
                case Language.Susu: return "sus";
                case Language.Swahili: return "swa";
                case Language.Swati: return "ssw";
                case Language.Swedish: return "swe";
                case Language.SwissGerman: return "gsw";
                case Language.Syriac: return "syr";
                case Language.Tagalog: return "tgl";
                case Language.Tahitian: return "tah";
                case Language.Tajik: return "tgk";
                case Language.Tamashek: return "tmh";
                case Language.Tamil: return "tam";
                case Language.Tatar: return "tat";
                case Language.Telugu: return "tel";
                case Language.Tereno: return "ter";
                case Language.Tetum: return "tet";
                case Language.Thai: return "tha";
                case Language.Tibetan: return "bod";
                case Language.Tigre: return "tig";
                case Language.Tigrinya: return "tir";
                case Language.Timne: return "tem";
                case Language.Tiv: return "tiv";
                case Language.Tlingit: return "tli";
                case Language.Tokelau: return "tkl";
                case Language.TokPisin: return "tpi";
                case Language.TongaNyasa: return "tog";
                case Language.TongaTongaIslands: return "ton";
                case Language.Tsimshian: return "tsi";
                case Language.Tsonga: return "tso";
                case Language.Tswana: return "tsn";
                case Language.Tumbuka: return "tum";
                case Language.Turkish: return "tur";
                case Language.Turkmen: return "tuk";
                case Language.Tuvalu: return "tvl";
                case Language.Tuvinian: return "tyv";
                case Language.Twi: return "twi";
                case Language.Udmurt: return "udm";
                case Language.Ugaritic: return "uga";
                case Language.Uighur: return "uig";
                case Language.Ukrainian: return "ukr";
                case Language.Umbundu: return "umb";
                case Language.Uncodedlanguages: return "mis";
                case Language.UpperSorbian: return "hsb";
                case Language.Urdu: return "urd";
                case Language.Uzbek: return "uzb";
                case Language.Vai: return "vai";
                case Language.Venda: return "ven";
                case Language.Vietnamese: return "vie";
                case Language.Votic: return "vot";
                case Language.Walloon: return "wln";
                case Language.Waray: return "war";
                case Language.Washo: return "was";
                case Language.Welsh: return "cym";
                case Language.WesternFrisian: return "fry";
                case Language.Wolaytta: return "wal";
                case Language.Wolof: return "wol";
                case Language.Xhosa: return "xho";
                case Language.Yakut: return "sah";
                case Language.Yao: return "yao";
                case Language.Yapese: return "yap";
                case Language.Yiddish: return "yid";
                case Language.Yoruba: return "yor";
                case Language.Zapotec: return "zap";
                case Language.Zaza: return "zza";
                case Language.Zenaga: return "zen";
                case Language.Zhuang: return "zha";
                case Language.Zulu: return "zul";
                case Language.Zuni: return "zun";
                default: return "und";
            }
        }

        public static VideoCodec GetVideoCodec(string Identifier)
        {
            switch(Identifier)
            {
                case "VC-1":
                case "V_MS/VFW/FOURCC":
                    return VideoCodec.VC1;
                case "Mpeg2":
                case "V_MPEG2":
                    return VideoCodec.MPEG2;
                case "Mpeg4":
                case "V_MPEG4":
                    return VideoCodec.MPEG4;
                case "Mpeg4-MVC-3D":
                    return VideoCodec.MPEG4MVC;
                case "MpegH":
                    return VideoCodec.MPEGHHVC;
                default: return VideoCodec.Unknown;
            }
        }

        public static AudioCodec GetAudioCodec(string Identifier, int Channels, StreamFlag StreamFlags)
        {
            bool isCore = StreamFlags.HasFlag(StreamFlag.CoreAudio);
            
            switch(Identifier)
            {
                case "TrueHD": return (Channels > 2) ? AudioCodec.TrueHD_Multi : AudioCodec.TrueHD_Stereo;
                case "DD": return (Channels > 2) ? ((isCore) ? AudioCodec.TrueHD_Core_Multi : AudioCodec.AC3_Multi) : ((isCore) ? AudioCodec.TrueHD_Core_Stereo : AudioCodec.AC3_Stereo);

                case "DTS-HD MA": return (Channels > 2) ? AudioCodec.DTSHDMasterAudio_Multi : AudioCodec.DTSHDMasterAudio_Stereo;
                case "DTS-HD HRA": return (Channels > 2) ? AudioCodec.DTSHD_Multi : AudioCodec.DTSHD_Stereo;
                case "DTS": return (Channels > 2) ? ((isCore) ? AudioCodec.DTSHD_Core_Multi : AudioCodec.DTS_Multi) : ((isCore) ? AudioCodec.DTSHD_Core_Stereo : AudioCodec.DTS_Stereo);

                case "LPCM": return (Channels > 2) ? AudioCodec.LPCM_Multi : AudioCodec.LPCM_Stereo;
                case "FLAC": return (Channels > 2) ? AudioCodec.FLAC_Multi : AudioCodec.FLAC_Stereo;

                // TODO: Not tested:
                case "MP2": return AudioCodec.MP2;
                case "MP3": return AudioCodec.MP3;
                case "EAC3": return (Channels > 2) ? AudioCodec.EAC3_Multi : AudioCodec.EAC3_Stereo;
                case "MLP": return (Channels > 2) ? AudioCodec.MLP_Multi : AudioCodec.MLP_Stereo;
                case "DTS-HD LBR": return (Channels > 2) ? AudioCodec.DTSHDLBR_Multi: AudioCodec.DTSHDLBR_Stereo;

                default: return AudioCodec.Unknown;
            }
        }

        public static StreamFlag GetAudioObjectAudioFlag(AudioCodec Codec, string TrackName, string CodecDescription)
        {
            switch(Codec)
            {
                case AudioCodec.TrueHD_Core_Multi:
                case AudioCodec.TrueHD_Multi:
                    if (!string.IsNullOrWhiteSpace(CodecDescription) && CodecDescription.Trim().Equals("TrueHD Atmos"))
                        return StreamFlag.HasObjectAudio;
                    else
                        return 0;
                case AudioCodec.DTSHDLBR_Multi:
                case AudioCodec.DTSHD_Core_Multi:
                case AudioCodec.DTSHD_Multi:
                case AudioCodec.DTSHDMasterAudio_Multi:
                    if (!string.IsNullOrWhiteSpace(TrackName) && TrackName.Contains("DTS:X"))
                        return StreamFlag.HasObjectAudio;
                    else
                        return 0;
                default:
                    return 0;
            }
        }

        public static string GetObjectAudioDescription(AudioCodec Codec)
        {
            switch (Codec)
            {
                case AudioCodec.TrueHD_Core_Multi:
                case AudioCodec.TrueHD_Multi:
                    return "Dolby Atmos";
                case AudioCodec.DTSHDLBR_Multi:
                case AudioCodec.DTSHD_Core_Multi:
                case AudioCodec.DTSHD_Multi:
                case AudioCodec.DTSHDMasterAudio_Multi:
                    return "DTS:X";
                default:
                    return null;
            }
        }

        public static SubtitleCodec GetSubtitleCodec(string Identifier)
        {
            switch (Identifier)
            {
                case "S_HDMV/PGS": return SubtitleCodec.PGS;
                case "S_VOBSUB": return SubtitleCodec.VOBSUB;
                case "S_VOBSUBHD": return SubtitleCodec.VOBSUBHD;
                default: return SubtitleCodec.Unknown;
            }
        }

        public static AudioChannelLayout GetAudioChannelLayout(string Identifier)
        {
            switch(Identifier)
            {
                case "mono":
                    return AudioChannelLayout.Mono;
                case "stereo":
                    return AudioChannelLayout.Stereo;
                case "5.1(rear)":
                    return AudioChannelLayout.Surround_51_Rear;
                case "5.1(side)":
                    return AudioChannelLayout.Surround_51_Side;
                case "7.1":
                    return AudioChannelLayout.Surround_71;
                default:
                    return AudioChannelLayout.Unknown;
            }
        }

        public static Size GetVideoResolution(string Identifier)
        {
            Match match = Regex.Match(Identifier, "^([0-9]+)x([0-9]+)$");
            Size size = new Size(0, 0);
            if (match.Success)
            {
                size.Width = int.Parse(match.Groups[1].Value);
                size.Height = int.Parse(match.Groups[2].Value);
            }
            return size;
        }

        public static double GetVideoAspectRatio(string Identifier)
        {
            switch(Identifier)
            {
                case "16:9": return 1.78;
                case "4:3": return 1.33;
                default: return 0;
            }
        }

        public static double GetVideoFramerate(string Identifier)
        {
            Match match = Regex.Match(Identifier, "^([0-9]+(?:\\.[0-9]+)?)");
            if (!match.Success) return 0;

            string value = match.Groups[1].Value;
            CultureInfo ci = new CultureInfo("en-US", false);
            double framerate;
            if (!double.TryParse(value, NumberStyles.AllowDecimalPoint, ci, out framerate)) framerate = 0;
            return framerate;
        }

        #endregion

    }

    public enum SourceType : byte
    {
        Folder = 1,
        File = 2,
        Disc = 3,
        Device = 4
    }

    public class Source
    {
        public event ProgressEventArgs.ProgressEventHandler ScanProgress;
        public event ScanEventArgs.ScanEventHandler ScanStarted;
        public event ScanEventArgs.ScanEventHandler ScanCompleted;
        public event ScanEventArgs.ScanEventHandler ScanFailed;

        public object Tag { get; set; }

        #region Hash

        private string hash = null;
        // A unique identifier (hash), identifying the source. Can be used to match previously scanned sources, fx to restore saved settings
        public string Hash
        {
            get
            {
                // Unique identifier can only be determined if source has been scanned successfully.
                if (scanResult != ScanResult.Success)
                    return null;

                if (hash == null)
                {
                    // Assemble source signature:
                    // 
                    // It should be specific enough that it is highly unlikely that two different sources have identical catalogs and thereby Md5 hashes.
                    // 
                    // <SOURCE>
                    //   <TYPE:type/>                 file / dvd / bluray
                    //   <NAME:name-default/>
                    //   <TITLE:index,source-filename,runtime(milliseconds),chapters,size(bytes),segment1-segment2-...>
                    //     <TRACK:type(video/audio/subtitle),(type specific)/>
                    //              video,codec,framerate,resolution(pixels)
                    //              audio,codec,language,channel-count,samplerate,bitdepth
                    //              subtitle,codec,language,forced(0/1)
                    //
                    // in case of tracks with children (3D video / core audio / forced only subtitles):
                    //
                    //     <TRACK:type(video/audio/subtitle),(type specific)>
                    //       <TRACK:type(video/audio/subtitle),(type specific)/>
                    //     </TRACK>
                    // 
                    //   </TITLE>
                    // </SOURCE>

                    StringBuilder sig = new StringBuilder();

                    sig.AppendLine("<SOURCE>");
                    sig.AppendFormat("\t<TYPE:{0}/>\r\n", disc.DiscType.ToString());
                    sig.AppendFormat("\t<NAME:{0}/>\r\n", disc.Name);

                    string sigTrack, sigTrackDerived;

                    foreach (Title title in titles)
                    {
                        sig.AppendFormat("\t<TITLE:{0}>\r\n", getTitleSignature(title));

                        foreach (Track track in title.Tracks)
                        {
                            sigTrack = getTrackSignature(track);
                            if (track.Child != null)
                            {
                                sigTrackDerived = getTrackSignature(track.Child);
                                sig.AppendFormat("\t\t<TRACK:{0}>\r\n\t\t\t<TRACK:{1}/>\r\n\t\t</TRACK>\r\n", sigTrack, sigTrackDerived);
                            }
                            else
                                sig.AppendFormat("\t\t<TRACK:{0}/>\r\n", sigTrack);
                        }

                        sig.AppendLine("\t</TITLE>");
                    }
                    sig.Append("</SOURCE>");

                    // NOTE:
                    // Settings are saved as three parts:
                    //      1/ Title/track settings (title/track names, include, filename, etc.)
                    //      2/ Viewstate (expanded/collapsed for each node)
                    //      3/ Output settings
                    //
                    // 1 + 3 is automatic persistent during application use, 2 is not.
                    // All 3 should be stored along with the hash generated here, when the application is exited, to be reloaded when it is run again.
                    // 2 should be stored in memory when selecting another source, and used to restore view state when the source is selected again.
                    //
                    // Along with the unique identifier, the source location is also stored. 
                    // This is used as a tie-breaker if the same source in multiple locations is scanned.
                    // But if there is only one source which matches the identifier, the location is disregarded (so settings will be restored even if the source is moved).
                    // If there are multiple sources matching (with different locations) and the location does not match, the newest source settings are used.

                    // Create Md5 hash on source signature generated above -> unique identifier (or highly unlikely that it's not)
                    using (MD5 md5 = MD5.Create())
                        hash = getMd5Hash(md5, sig.ToString());

                    //Debug.WriteLine("SIGNATURE:");
                    //Debug.WriteLine(sig.ToString());
                    //Debug.WriteLine("HASH: " + hash);
                }

                return hash;
            }
        }

        private static string getTitleSignature(Title title)
        {
            // index,source-filename,runtime(milliseconds),chapters,size(bytes),segment1-segment2-...
            string segments;
            if (title.Segments != null && title.Segments.GetLength(0) > 0)
            {
                segments = "";
                foreach (int segment in title.Segments)
                {
                    if (segments.Length > 0) segments += "-";
                    segments += segment.ToString();
                }
            }
            else
                segments = "-";
            
            return String.Format("{0},{1},{2},{3},{4},{5}", 
                title.Index, 
                title.SourceFilename, 
                title.Duration.TotalMilliseconds, 
                title.Chapters,
                title.Size, 
                segments);
        }

        private static string getTrackSignature(Track track)
        {
            // video,codec,framerate,resolution(pixels)
            // audio,codec,language,channel-count,samplerate,bitdepth
            // subtitle,codec,language,forced(0/1)
            switch (track.TrackType)
            {
                case TrackType.Video:
                    NumberFormatInfo nfi = new NumberFormatInfo();
                    nfi.NumberDecimalSeparator = ".";
                    return String.Format("video,{0},{1},{2}",
                        track.VideoCodec.ToString(),
                        track.VideoFramerate.ToString("0.000", nfi),
                        track.VideoResolution.Width * track.VideoResolution.Height);
                case TrackType.Audio:
                    return String.Format("audio,{0},{1},{2},{3},{4}",
                        track.AudioCodec.ToString(),
                        MakeMKV.GetLanguageISOCode(track.Language),
                        track.AudioChannels.ToString(),
                        track.AudioSampleRate.ToString(),
                        track.AudioBitdepth.ToString());
                case TrackType.Subtitle:
                    return String.Format("subtitle,{0},{1},{2}",
                        track.SubtitleCodec.ToString(),
                        MakeMKV.GetLanguageISOCode(track.Language),
                        (track.SubtitleForced ? "1" : "0"));
            }

            return "";
        }

        private static string getMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++) sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        #endregion

        private void OnScanProgressEvent(object sender, ProgressEventArgs e)
        { if (ScanProgress != null) ScanProgress(sender, e); }

        private void OnScanStartedEvent(object sender, ScanEventArgs e)
        { if (ScanStarted != null) ScanStarted(sender, e); }

        private void OnScanCompletedEvent(object sender, ScanEventArgs e)
        { if (ScanCompleted != null) ScanCompleted(sender, e); lock (scanLock) { scanInProgress = false; scanResult = e.ScanResult; } }

        private void OnScanFailedEvent(object sender, ScanEventArgs e)
        { if (ScanFailed != null) ScanFailed(sender, e); lock (scanLock) { scanInProgress = false; scanResult = e.ScanResult; } }

        private SourceType type;
        public SourceType Type { get { return type; } }

        private string location;
        public string Location { get { return location; } }

        private Disc disc;
        public Disc Disc { get { return disc; } }

        private byte driveIndex = 255;
        public string Identifier
        {
            get 
            { 
                switch(type)
                {
                    case SourceType.File: 
                        if (location.EndsWith(".iso", StringComparison.OrdinalIgnoreCase))
                            return "iso:\"" + location + "\"";
                        else
                            return "file:\"" + location + "\"";
                    case SourceType.Folder: return "file:\"" + location + "\"";
                    case SourceType.Disc:
                        // Location is the path (fx E:\), determine the corresponding disc ID.
                        if (driveIndex == 255)
                            driveIndex = makeMKV.GetDriveIndex(location);
                        return (driveIndex < 255 ? string.Format("disc:{0}", driveIndex) : null);
                    case SourceType.Device: return "dev:\"" + location + "\"";
                    default: return null;
                }
            }
        }

        private MakeMKV makeMKV;
        private MKVToolNix mkvToolNix;

        public Source(MakeMKV MakeMKV, SourceType Type, string Location, Disc Disc, List<string> ScanInfo, MKVToolNix MKVToolNix = null)
        {
            // Initializes the source with info from a previous scan, the source does not need to be scanned.

            this.makeMKV = MakeMKV;
            this.mkvToolNix = MKVToolNix;
            this.type = Type;
            this.location = Location.Trim();

            this.disc = new Disc(Disc.DiscType, Disc.Name, Disc.MetadataLanguage, this);
            Title title;
            List<Title> result = new List<Title>();
            foreach (string scanInfo in ScanInfo)
            {
                title = new Title(this, disc, scanInfo.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>(), makeMKV, mkvToolNix);
                if (title.Index < 0) continue;

                title.OutputFolder = makeMKV.DefaultOutputFolder;
                result.Add(title);
            }
            titles = result.ToArray();

            scanResult = ScanResult.Success;
        }

        public Source(MakeMKV MakeMKV, SourceType Type, string Location, MKVToolNix MKVToolNix = null)
        {
            // Initializes the source from scratch (no info from a previous scan), the source needs to be scanned.

            this.makeMKV = MakeMKV;
            this.mkvToolNix = MKVToolNix;
            this.type = Type;
            this.location = Location.Trim();
        }

        public void AbortScan()
        {
            lock (scanLock)
            {
                if (scanInProgress && process != null) { cancelRequested = true; if (!process.HasExited) process.Kill(); }
            }
        }

        public bool IsIdentical(Source Source)
        {
            return IsIdentical(Source.type, Source.Location);
        }

        public bool IsIdentical(SourceType SourceType, string SourceIdentifier)
        {
            if (SourceType != type) return false;
            return (this.location.Equals(SourceIdentifier.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }

        private ScanResult scanResult = ScanResult.NotAvailable;
        public ScanResult Result { get { return scanResult; } }

        private Title[] titles;
        public Title[] Titles { get { return titles; } }

        #region Scan

        private object scanLock = new object();
        private Process process;
        private bool cancelRequested, scanInProgress, scanComplete, processExited;
        private List<string> stdout;
        private ProgressEventArgs scanEventArgs;
        private DateTime scanStarted;

        private bool scanUsingJava;
        public bool ScannedUsingJava { get { return scanUsingJava; } }

        public void Scan()
        {
            lock (scanLock)
            {
                if (scanInProgress) 
                {
                    if (makeMKV.EventLogger != null) makeMKV.EventLogger.LogEntry("MakeMKV", "makemkvcon cannot start new scan, scan already in progress.", EventLogEntryType.Warning);
                    OnScanFailedEvent(this, new ScanEventArgs("Scan already in progress.", ScanResult.ScanAlreadyInProgress));
                    return;
                }

                scanInProgress = true;
                cancelRequested = false;
                scanStarted = DateTime.Now;
                scanComplete = false;
                processExited = false;
                scanUsingJava = false;
                titles = null;
                scanResult = ScanResult.ScanInProgress;
            }

            ScanEventArgs startedEventArgs = new ScanEventArgs("Scan started.");
            OnScanStartedEvent(this, startedEventArgs);

            if (startedEventArgs.Cancel) 
            { OnScanFailedEvent(this, new ScanEventArgs("Cancelled by user.", ScanResult.CancelledByUser)); return; }

            string cmd;
            bool is64bit = Environment.Is64BitOperatingSystem;
            cmd = makeMKV.MakeMKVPath + "\\makemkvcon" + (is64bit ? "64" : "") + ".exe";

            if (!File.Exists(cmd)) 
            {
                if (makeMKV.EventLogger != null) makeMKV.EventLogger.LogEntry("MakeMKV", string.Format("makemkvcon executable not found.\r\n\r\nPath: {0}", cmd), EventLogEntryType.Error);
                OnScanFailedEvent(this, new ScanEventArgs(cmd + " not found.", ScanResult.MakeMKVConverterNotFound)); return;
            }

            string identifier = this.Identifier;
            if (identifier == null)
            { OnScanFailedEvent(this, new ScanEventArgs(String.Format("Drive {0} not found.", this.Location), ScanResult.DriveNotFound)); return; }

            string args = "-r --noscan --minlength=" + makeMKV.MinimumTitleLength.ToString() + " --messages=-stdout --progress=-same info " + identifier;

            stdout = new List<string>();
            discInfo = new List<List<string>>();
            scanEventArgs = new ProgressEventArgs();

            process = new Process();

            // Debug.WriteLine("-----------------------------------------------------------");
            // Debug.WriteLine("{0} {1}", cmd, args);

            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = args;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(process_Exited);

            try { process.Start(); }
            catch {
                if (makeMKV.EventLogger != null) makeMKV.EventLogger.LogEntry("MakeMKV", string.Format("makemkvcon failed to execute.\r\n\r\nExecutable: {0}\r\nArguments: {1}", process.StartInfo.FileName, process.StartInfo.Arguments), EventLogEntryType.Error);
                OnScanFailedEvent(this, new ScanEventArgs("Could not start MakeMKV.", ScanResult.CouldNotStartMakeMKV));
                return;
            }

            new MethodInvoker(ReadStdOut).BeginInvoke(null, null);
        }

        private void process_Exited(object sender, EventArgs e)
        {
            lock (scanLock) { if (cancelRequested) { OnScanFailedEvent(this, new ScanEventArgs("Cancelled by user.", ScanResult.CancelledByUser)); return; } }
            if (!scanComplete)
            {
                if (makeMKV.EventLogger != null)
                    makeMKV.EventLogger.LogEntry("MakeMKV", string.Format("makemkvcon scan did not complete.\r\n\r\nExecutable: {0}\r\nArguments: {1}\r\n\r\nOutput:\r\n\r\n{2}", process.StartInfo.FileName, process.StartInfo.Arguments, stdout.Aggregate((i, j) => i + "\r\n" + j)), EventLogEntryType.Error);
                OnScanFailedEvent(this, new ScanEventArgs("Scan not completed.", ScanResult.ScanNotCompleted));
                return;
            }

            // Ensure that process is fully exited.
            Thread.Sleep(500);
            byte b = 0;
            while (!process.HasExited && b < 6)
            {
                try { process.Kill(); }
                catch { }
                Thread.Sleep(500);
                b++;
            }
            process.Close();
            lock (scanLock) processExited = true;

            disc = new Disc(discType, discName, discMetadataLanguage, this);
            Title title;
            List<Title> result = new List<Title>();
            foreach (List<string> scanInfo in discInfo)
            {
                title = new Title(this, disc, scanInfo, makeMKV, mkvToolNix);
                if (title.Index < 0) continue;

                title.OutputFolder = makeMKV.DefaultOutputFolder;
                result.Add(title);
            }

            titles = result.ToArray();
            TimeSpan duration = DateTime.Now - scanStarted;
            scanResult = ScanResult.Success;
            OnScanCompletedEvent(this, new ScanEventArgs("Scan completed in " + duration.ToString(@"hh\:mm\:ss") + ", " + result.Count + " title" + ((result.Count != 1) ? "s" : "") + " found.", ScanResult.Success));
            if (makeMKV.EventLogger != null)
                makeMKV.EventLogger.LogEntry("MakeMKV", string.Format("makemkvcon scan completed successfully.\r\n\r\nExecutable: {0}\r\nArguments: {1}\r\n\r\nOutput:\r\n\r\n{2}", process.StartInfo.FileName, process.StartInfo.Arguments, stdout.Aggregate((i, j) => i + "\r\n" + j)), EventLogEntryType.Information);

        }

        private DiscType discType;
        private string discName;
        private Language discMetadataLanguage;

        List<List<string>> discInfo;
        public List<List<string>> ScanInfo
        { get { return discInfo; } }

        void ReadStdOut()
        {
            string line;
            lock (stdout)
            {
                while ((line = process.StandardOutput.ReadLine()) != null && !processExited)
                {
                    //Debug.WriteLine(line);

                    stdout.Add(line);
                    string value;
                    Match match = Regex.Match(line, "^([A-Z]{3,5}):([0-9]+),([0-9]+),(.*)$");
                    if (match.Success)
                    {
                        switch (match.Groups[1].Value)
                        {
                            case "PRGC":
                            case "PRGT":
                                value = match.Groups[4].Value;
                                value = value.Substring(1, value.Length - 2).Trim();
                                lock (scanEventArgs) { if (match.Groups[1].Value == "PRGC") { scanEventArgs.CurrentProgressTitle = value; } else { scanEventArgs.TotalProgressTitle = value; } }
                                OnScanProgressEvent(this, scanEventArgs);
                                break;
                            case "PRGV":
                                lock (scanEventArgs)
                                {
                                    scanEventArgs.CurrentProgress = double.Parse(match.Groups[2].Value) / 65536;
                                    scanEventArgs.TotalProgress = double.Parse(match.Groups[3].Value) / 65536;
                                }
                                OnScanProgressEvent(this, scanEventArgs);
                                break;
                            case "CINFO":
                                value = match.Groups[4].Value;
                                value = value.Substring(1, value.Length - 2).Trim();
                                switch (int.Parse(match.Groups[2].Value))
                                {
                                    case 1:
                                        switch(int.Parse(match.Groups[3].Value))
                                        {
                                            case 6209:
                                                discType = DiscType.Bluray;
                                                break;
                                            case 6206:
                                                discType = DiscType.DVD;
                                                break;
                                            default:
                                                discType = DiscType.File;
                                                break;
                                        }
                                        break;
                                    case 2:
                                        discName = value;
                                        break;
                                    case 28:
                                        discMetadataLanguage = MakeMKV.GetLanguage(value);
                                        break;
                                }
                                break;
                            case "TINFO":
                            case "SINFO":
                                int index = int.Parse(match.Groups[2].Value);
                                lock (discInfo)
                                {
                                    while (discInfo.Count < index + 1) discInfo.Add(new List<string>());
                                    discInfo[index].Add(line);
                                }
                                break;
                            case "MSG":
                                switch (int.Parse(match.Groups[2].Value))
                                {
                                    case 3344:
                                        // Using Java runtime.
                                        if (match.Groups[4].Value.Contains("Java runtime"))
                                            scanUsingJava = true;
                                        break;
                                    case 5011:
                                        // Operation successfully completed.
                                        lock (scanLock) { scanComplete = true; }
                                        break;
                                }
                                break;
                        }
                    }
                }
            }

            string cmp = string.Join("\r\n", stdout);
            Console.Write(cmp);
        }

        public enum ScanResult
        {
            NotAvailable = 1,
            Success = 0,
            UnspecifiedError = -1,
            MakeMKVConverterNotFound = -3,
            CancelledByUser = -6,
            ScanNotCompleted = -8,
            CouldNotStartMakeMKV = -10,
            ScanAlreadyInProgress = -11,
            ScanInProgress = -12,
            DriveNotFound = -13
        }

        #endregion

    }

    public class Disc
    {
        private DiscType discType;
        public DiscType DiscType { get { return discType; } }

        private string name;
        public string Name { get { return name; } }

        private Language metadataLanguage;
        public Language MetadataLanguage { get { return metadataLanguage; } }

        private Source source;
        public Source Source { get { return source; } }

        public Disc(DiscType DiscType, string Name, Language MetadataLanguage, Source Source)
        {
            this.discType = DiscType;
            this.name = Name;
            this.metadataLanguage = MetadataLanguage;
            this.source = Source;
        }
    }

    public class Title
    {

        #region Events

        public event ProgressEventArgs.ProgressEventHandler ConversionProgress;
        public event ConversionEventArgs.ConversionEventHandler ConversionStarted;
        public event ConversionEventArgs.ConversionEventHandler ConversionCompleted;
        public event ConversionEventArgs.ConversionEventHandler ConversionFailed;

        private void OnConversionStartedEvent(ConversionEventArgs e)
        { if (ConversionStarted != null) ConversionStarted(this, e); }

        private void OnConversionProgressEvent(ProgressEventArgs e)
        { if (ConversionProgress != null) ConversionProgress(this, e); }

        private void OnConversionCompletedEvent(ConversionEventArgs e)
        {
            lock (conversionLock) { conversionInProgress = false; result = e.ConversionResult; }
            if (overrideSelectionStringInRegistry) makeMKV.RestoreDefaultSelectionString();
            if (ConversionCompleted != null) ConversionCompleted(this, e); 
        }

        private void OnConversionFailedEvent(ConversionEventArgs e)
        {
            lock (conversionLock) { conversionInProgress = false; result = e.ConversionResult; }
            if (overrideSelectionStringInRegistry) makeMKV.RestoreDefaultSelectionString();
            if (ConversionFailed != null) ConversionFailed(this, e); 
        }

        #endregion

        public Title(Source Source, Disc Disc, List<string> ScanInfo, MakeMKV MakeMKV, MKVToolNix MKVToolNix = null)
        {

            // ScanInfo is all lines from makemkvcon for this title.
            this.source = Source;
            this.disc = Disc;
            this.makeMKV = MakeMKV;
            this.mkvToolNix = MKVToolNix;
            this.include = true;
            this.comment = null;

            // string si = String.Join("\r\n", ScanInfo);

            Regex regex = new Regex("^(TINFO|SINFO):([0-9]+),([0-9]+),([0-9]+),(?:[0-9]+,)?\"(.*)\"$");
            Match match;

            List<List<string>> trackInfo = new List<List<string>>();
            int id;
            string value, valueRaw;

            index = -1;

            if (ScanInfo == null || ScanInfo.Count == 0)
                return;

            foreach(string line in ScanInfo)
            {
                match = regex.Match(line);
                if (!match.Success || match.Groups.Count < 6)
                    continue;

                if (index < 0 && !int.TryParse(match.Groups[2].Value, out index))
                    break;

                if (!int.TryParse(match.Groups[3].Value, out id))
                    continue;

                switch (match.Groups[1].Value)
                {
                    case "TINFO":
                        valueRaw = (!String.IsNullOrWhiteSpace(match.Groups[5].Value) ? match.Groups[5].Value : "");
                        value = valueRaw.Trim();
                        switch(id)
                        {
                            case 2:
                                name = value;
                                nameDefault = value;
                                break;
                            case 8:
                                if (!byte.TryParse(value, out chapters)) size = 0;
                                break;
                            case 9:
                                if (!TimeSpan.TryParse(value, out duration)) duration = new TimeSpan();
                                break;
                            case 11:
                                if (!long.TryParse(value, out size)) size = 0;
                                break;
                            case 16:
                                sourceFilename = value.Replace("\\\\", "\\");
                                break;
                            case 26:
                                string[] sgmt = value.Split(',');
                                segments = new int[sgmt.GetLength(0)];
                                for (int s = 0; s < sgmt.GetLength(0); s++) int.TryParse(sgmt[s], out segments[s]);
                                break;
                            case 27:
                                outputFilenameDefault = Common.SanitizeFilename(value);
                                tempFilename = outputFilenameDefault;
                                outputFilename = outputFilenameDefault;
                                break;
                            case 28:
                                metadataLanguage = MakeMKV.GetLanguage(value);
                                break;
                            case 49:
                                comment = value;
                                break;
                        }
                        break;
                    case "SINFO":
                        while (trackInfo.Count < id + 1) trackInfo.Add(new List<string>());
                        trackInfo[id].Add(line);
                        break;
                }

            }

            tracks = new List<Track>();
            tracksAll = new List<Track>();
            Track track, prevTrack = null;
            foreach (List<string> scanInfo in trackInfo)
            {
                track = new Track(scanInfo, this);
                if (track.Index < 0) continue;

                tracksAll.Add(track);

                if (prevTrack != null && prevTrack.IsValidChild(track))
                {
                    prevTrack.Child = track;
                    track.Parent = prevTrack;
                    prevTrack = null;
                }
                else
                {
                    tracks.Add(track);
                    prevTrack = track;
                }
            }

            scanInfo = ScanInfo.Aggregate((c, n) => c + "\r\n" + n);
        }

        #region Properties

        private ConversionResult result = ConversionResult.NotAvailable;
        public ConversionResult Result { get { return result; } }

        private string scanInfo;
        public string ScanInfo { get { return scanInfo; } }

        public bool ResetResult(ConversionResult Result = ConversionResult.NotAvailable)
        {
            lock(conversionLock)
            {
                if (conversionInProgress) return false;
                this.result = Result;
                return true;
            }
        }

        private MakeMKV makeMKV;
        public MakeMKV MakeMKV { get { return makeMKV; } set { makeMKV = value; } }

        private MKVToolNix mkvToolNix;
        public MKVToolNix MKVToolNix { get { return mkvToolNix; } set { mkvToolNix = value; } }

        private Source source;
        public Source Source { get { return source; } }

        private Disc disc;
        public Disc Disc { get { return disc; } }

        private bool include;
        public bool Include { get { return include; } set { include = value; } }

        private string tempFilename;

        private string outputFolderTemplate;
        public string OutputFolderTemplate
        {
            get { return outputFolderTemplate; }
            set {
                outputFolderTemplate = value;
                outputFolder = getProcessedName(this.Source, outputFolderTemplate).TrimEnd('\\');
            }
        }

        private string outputFolder;
        public string OutputFolder { 
            get
            {
                // NOTE: Output folder is not updated using template here, because the allowed tags for output folder cannot have changed since the template was set.
                return outputFolder;
            } 
            set {
                if (value != OutputFolder)
                {
                    outputFolderTemplate = null; // Reset template if output folder was specified explicitly
                    outputFolder = value.Trim().TrimEnd('\\');
                }
            } 
        }

        private string outputFilenameTemplate;
        public string OutputFilenameTemplate
        {
            get { return outputFilenameTemplate; }
            set
            {
                if (value != outputFilenameTemplate)
                {
                    outputFilenameTemplate = value;
                    outputFilenameBase = null;
                }
            }
        }

        private string outputFilenameDefault;
        public string OutputFilenameDefault { get { return outputFilenameDefault; } }

        private string outputFilenameBase;
        public string OutputFilenameBase // Like OutputFilename but without extension (.mkv)
        {
            get
            {
                if (outputFilenameBase == null)
                {
                    outputFilenameBase = OutputFilename;
                    if (outputFilenameBase.EndsWith(".mkv", StringComparison.InvariantCultureIgnoreCase))
                        outputFilenameBase = outputFilenameBase.Substring(0, outputFilenameBase.Length - 4);
                }

                return outputFilenameBase;
            }
        }

        private string outputFilename;
        public string OutputFilename {
            get
            {
                // Update outputFilename using template.
                if (outputFilenameTemplate != null)
                {
                    outputFilename = Common.SanitizeFilename(getProcessedName(this, outputFilenameTemplate));
                    if (outputFilename.Length > 204)
                        outputFilename = outputFilename.Substring(0, 200) + ".mkv";
                }

                return ((string.IsNullOrWhiteSpace(outputFilename) || !outputFilename.EndsWith(".mkv", StringComparison.InvariantCultureIgnoreCase) || outputFilename.StartsWith(".mkv", StringComparison.InvariantCultureIgnoreCase)) ? outputFilenameDefault : outputFilename);
            }
            set
            {
                if (value != null && value != OutputFilename)
                {
                    outputFilenameTemplate = null; // Reset template if output filename was specified explicitly
                    outputFilename = value.Trim();
                    if (outputFilename.Length > 204)
                        outputFilename = outputFilename.Substring(0, 200) + ".mkv";
                    outputFilenameBase = null;
                }
            }
        }

        public string OutputFullName { get { return (OutputFolder != null && OutputFilename != null ? OutputFolder + '\\' + OutputFilename : null); } }

        public string OutputFullNameBase { get { return (OutputFolder != null && OutputFilenameBase != null ? OutputFolder + '\\' + OutputFilenameBase : null); } }

        private List<Track> tracks;
        public List<Track> Tracks { get { return tracks; } }

        private List<Track> tracksAll;
        public List<Track> TracksAll { get { return tracksAll; } }

        public List<Track> GetTracksByType(TrackType Type)
        { return tracks.Where(t => t.TrackType == Type).ToList<Track>(); }


        private int index;
        public int Index { get { return index; } }

        private string nameDefault;
        public string NameDefault { get { return (nameDefault != null ? nameDefault : ""); } }

        private string nameTemplate;
        public string NameTemplate
        {
            get { return nameTemplate; }
            set { nameTemplate = value; }
        }

        private string name; // Modification must be processed using mkvpropedit after conversion.
        public string Name { 
            get
            {
                // Update name using template.
                if (nameTemplate != null)
                    name = Regex.Replace(getProcessedName(this, nameTemplate), "[^\\w.,:;&%'!?#()\\[\\]\\-_\" ]", "");

                return name;
            } 
            set {
                if (value != null && value != Name)
                {
                    // Check if output filename should be set based on name.
                    if ((outputFilename == outputFilenameDefault && (OutputFilenameTemplate == null || OutputFilenameTemplate == "")) ||
                        (OutputFilenameTemplate != null && OutputFilenameTemplate == nameTemplate + ".mkv") ||
                        (OutputFilename == name + ".mkv"))
                    {
                        string filename = Common.SanitizeFilename(value);
                        if (filename.Length > 0) OutputFilename = filename + ".mkv";
                    }

                    name = value;
                    nameTemplate = null; // Reset template if name was specified explicitly
                }
            } 
        }

        public bool NameModified { get { return (Name != nameDefault); } }

        public bool VideoInclude3D {
            get
            {
                // Returns true if there is a MPEG-MVC-3D track and it is selected to include.
                Track track = tracks.Where(t => t.TrackType == TrackType.Video).FirstOrDefault<Track>();
                return (track != null && track.Child != null && track.Child.VideoCodec == VideoCodec.MPEG4MVC && track.Child.Include);
            }
        }

        private Language metadataLanguage;
        public Language MetadataLanguage { get { return metadataLanguage; } set { metadataLanguage = value; } }

        private string sourceFilename;
        public string SourceFilename { get { return sourceFilename; } }

        private byte chapters;
        public byte Chapters { get { return chapters; } }

        private TimeSpan duration;
        public TimeSpan Duration { get { return duration; } }

        private long size;
        public long Size { get { return size; } }

        private long outputSize = 0;
        public long OutputSize { get { return outputSize; } }

        private int[] segments;
        public int[] Segments { get { return segments; } }

        private string comment;
        public string Comment { get { return comment; } }

        #endregion

        #region Conversion

        private object conversionLock = new object();
        private Process process;
        private ProgressEventArgs conversionEventArgs;
        private bool copyComplete;
        private string tmpFolder, profile;
        private bool conversionInProgress = false;
        private DateTime conversionStarted;
        private bool cancelRequested = false;
        private bool finishedReadingOutput = false;

        private List<int> tracksIgnored;
        private List<Track> tracksIncluded;

        public void AbortConversion()
        {
            lock(conversionLock)
            {
                if (conversionInProgress && process != null)
                {
                    cancelRequested = true;
                    if (!process.HasExited)
                    {
                        try { process.Kill(); }
                        catch { }
                    }
                }
            }
        }

        private readonly bool overrideSelectionStringInRegistry = true;
        public void Convert()
        {
            lock(conversionLock)
            {
                if (conversionInProgress) 
                { OnConversionFailedEvent(new ConversionEventArgs("Conversion already in progress.", ConversionResult.ConversionAlreadyInProgress)); return; }

                conversionInProgress = true;
                cancelRequested = false;
                copyComplete = false;
                result = ConversionResult.ConversionInProgress;
                conversionStarted = DateTime.Now;
            }

            this.outputSize = 0;

            // Converts the title to an mkv-file in OutputFolder named OutputFilename.
            ConversionEventArgs startedEventArgs = new ConversionEventArgs("Copy started.");
            OnConversionStartedEvent(startedEventArgs);

            if (startedEventArgs.Cancel) { OnConversionFailedEvent(new ConversionEventArgs("Cancelled.", ConversionResult.CancelledByUser)); return; }

            // Check if outputFolder is specified
            if (String.IsNullOrWhiteSpace(outputFolder)) { OnConversionFailedEvent(new ConversionEventArgs("Output folder not specified.", ConversionResult.OutputFolderNotSpecified)); return; }

            // Check that output filename length does not exceed maximum length.
            if (OutputFilename.Length > 120 || OutputFullName.Length > 250) { OnConversionFailedEvent(new ConversionEventArgs("Output filename and/or path exceeds maximum length.", ConversionResult.OutputPathOrFilenameInvalid)); return; }

            // Check that output filename has .mkv extension.
            if (!OutputFilename.EndsWith(".mkv", StringComparison.InvariantCultureIgnoreCase)) { OnConversionFailedEvent(new ConversionEventArgs("Output filename does not have .mkv extension.", ConversionResult.OutputPathOrFilenameInvalid)); return; }

            // Check that makemkvcon path is valid.
            bool is64bit = Environment.Is64BitOperatingSystem;
            string cmd = makeMKV.MakeMKVPath + "\\makemkvcon" + (is64bit ? "64" : "") + ".exe";
            if (!File.Exists(cmd)) { OnConversionFailedEvent(new ConversionEventArgs(cmd + " not found.", ConversionResult.OutputFolderNotSpecified)); return; }

            // Get identifier, check that it's available.
            string identifier = disc.Source.Identifier;
            if (String.IsNullOrWhiteSpace(identifier))
            { OnConversionFailedEvent(new ConversionEventArgs(String.Format("Drive {0} not found.", disc.Source.Location), ConversionResult.DriveNotFound)); return; }

            // Check if outputFolder exists, attempt to create if it does not.
            if (!Directory.Exists(OutputFolder))
            {
                try { Directory.CreateDirectory(OutputFolder); }
                catch { OnConversionFailedEvent(new ConversionEventArgs("Could not access or create output folder: " + OutputFolder, ConversionResult.CouldNotAccessOrCreateOutputFolder)); return; }
            }
            
            // Create temp folder.
            tmpFolder = OutputFolder + "\\~Temp";
            if (!Directory.Exists(tmpFolder))
            {
                try { Directory.CreateDirectory(tmpFolder); }
                catch { OnConversionFailedEvent(new ConversionEventArgs("Could not access or create temporary output folder: " + tmpFolder, ConversionResult.CouldNotAccessOrCreateTemporaryFolder)); return; }
            }

            // Create conversion profile
            profile = tmpFolder + "\\" + tempFilename.Substring(0, tempFilename.Length - 3) + "mmcp.xml";
            string selection;
            if (!this.CreateConversionProfile(profile, out tracksIncluded, out selection)) { cleanup(profile, tmpFolder); OnConversionFailedEvent(new ConversionEventArgs("Could not save conversion profile: " + profile, ConversionResult.CouldNotCreateConversionProfile)); return; }

            // Prepare arguments
            // makemkvcon64 -r --profile=(profilePath) mkv (source) (title.Index) (outputFolder/~Temp)
            string args = "-r --noscan --minlength=" + MakeMKV.MinimumTitleLength.ToString() + " --messages=-stdout --progress=-same --profile=\"" + profile + "\" mkv " + disc.Source.Identifier + " " + this.index.ToString() + " \"" + tmpFolder + "\"";
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine(cmd + " " + args);

            // Backup, then clear, default selection string, so value in custom conversion profile is used.
            if (overrideSelectionStringInRegistry && 
                (!makeMKV.BackupDefaultSelectionString() || !makeMKV.SetDefaultSelectionString(selection)))
            {
                OnConversionFailedEvent(new ConversionEventArgs("Could not modify default selection string.", ConversionResult.CouldNotModifySelectionString)); return;
            }

            // Run makemkvcon as process on a separate thread.
            conversionEventArgs = new ProgressEventArgs(this.Name);
            process = new Process();
            copyComplete = false;
            finishedReadingOutput = false;
            tracksIgnored = new List<int>(); // 0-based indices of tracks that were empty/ignored by MakeMKV and not included in the output file. Only tracks included in the conversion are counted.

            // Redirect the output stream of the child process.
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = args;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(process_Exited);

            try { process.Start(); }
            catch { cleanup(profile, tmpFolder); OnConversionFailedEvent(new ConversionEventArgs("Could not start MakeMKV.", ConversionResult.CouldNotStartMakeMKV)); return; }

            new MethodInvoker(ReadStdOut).BeginInvoke(null, null);
            mkvToolNix = MKVToolNix;
        }

        private bool tracksAsExpected(List<Track> Expected, MKVFile.Track[] Actual)
        {
            if (Expected == null || 
                Actual == null) 
                return false;

            int j = 0, n = Actual.GetLength(0);
            for (int i = 0; i < Expected.Count; i++)
            {
                if (!Expected[i].IsEmpty)
                {
                    // Check track exists
                    if (n <= j) return false;

                    // Check track type
                    if (Expected[i].TrackType != Actual[j].Type) return false;

                    // Check track language (ignore if video or if expected language was not known)
                    if (Actual[j].Type != TrackType.Video &&
                        Expected[i].Language != Language.Undetermined &&
                        Expected[i].Language != Actual[j].Language)
                        return false;

                    // Check codec
                    switch (Expected[i].TrackType)
                    {
                        case TrackType.Video:
                            switch (Expected[i].VideoCodec)
                            {
                                case VideoCodec.MPEG2:
                                    if (!Actual[j].CodecID.StartsWith("V_MPEG2")) return false; break;
                                case VideoCodec.MPEG4:
                                    if (!Actual[j].CodecID.StartsWith("V_MPEG4")) return false; break;
                                case VideoCodec.VC1:
                                    if (!Actual[j].CodecID.StartsWith("V_MS")) return false; break;
                                case VideoCodec.MPEG4MVC:
                                    if (!Actual[j].CodecID.StartsWith("V_MPEG4/MVC")) return false; break;

                                    // TODO: ...
                            }
                            break;
                        case TrackType.Audio:
                            switch (Expected[i].AudioOutputFormat(this.MakeMKV))
                            {
                                case AudioOutputFormat.DirectCopy:
                                    switch (Expected[i].AudioCodec)
                                    {
                                        case AudioCodec.DTSHDMasterAudio_Multi:
                                        case AudioCodec.DTSHDMasterAudio_Stereo:
                                        case AudioCodec.DTSHD_Core_Multi:
                                        case AudioCodec.DTSHD_Core_Stereo:
                                        case AudioCodec.DTS_Multi:
                                        case AudioCodec.DTS_Stereo:
                                            if (!Actual[j].CodecID.StartsWith("A_DTS")) return false; break;

                                        // TODO: ...
                                    }
                                    break;
                                case AudioOutputFormat.FLAC:
                                    if (!Actual[j].CodecID.StartsWith("A_FLAC")) return false; break;
                                case AudioOutputFormat.LPCM:
                                    if (!Actual[j].CodecID.StartsWith("A_PCM")) return false; break;
                            }
                            break;
                        case TrackType.Subtitle:
                            switch (Expected[i].SubtitleCodec)
                            {
                                case SubtitleCodec.PGS:
                                    if (!Actual[j].CodecID.Equals("S_HDMV/PGS")) return false; break;
                                case SubtitleCodec.VOBSUB:
                                    if (!Actual[j].CodecID.Equals("S_VOBSUB")) return false; break;
                                case SubtitleCodec.VOBSUBHD:
                                    if (!Actual[j].CodecID.Equals("S_VOBSUBHD")) return false; break;
                            }
                            break;
                    }

                    j++;
                }
            }

            return (j == n);
        }

        private void process_Exited(object sender, EventArgs e)
        {
            // Ensure that standard output has been read fully before proceeding.
            // Wait at least 0.5 secs, but not more than 5 secs.
            Thread.Sleep(500);
            byte b = 0;
            while (!process.HasExited && b < 11)
            {
                if (!process.HasExited)
                {
                    // If process has not exited, attempt to kill it.
                    try { process.Kill(); }
                    catch { }
                }
                else
                {
                    // If process has exited, make sure that all output has been read before proceeding.
                    lock (conversionLock)
                        if (finishedReadingOutput) break;
                }
                Thread.Sleep(500);
                b++;
            }
            process.Close();

            lock (conversionLock) 
            { 
                if (cancelRequested) 
                { 
                    cleanup(profile, tmpFolder); 
                    OnConversionFailedEvent(new ConversionEventArgs("Cancelled.", ConversionResult.CancelledByUser)); 
                    return; 
                }

                if (!copyComplete)
                {
                    cleanup(profile, tmpFolder);
                    OnConversionFailedEvent(new ConversionEventArgs("Copy failed.", ConversionResult.MakeMKVCopyFailed));
                    return;
                }
            }

            // Delete conversion profile
            try { File.Delete(profile); }
            catch { /* fail silently - not a critical error */ }

            // If any tracks turned out to be empty, mark them here.
            for (int i = 0; i < tracksIgnored.Count; i++)
                if (tracksIncluded.Count > tracksIgnored[i])
                    tracksIncluded[tracksIgnored[i]].IsEmpty = true;

            bool mkvFileCorrected = false;
            if (mkvToolNix != null)
            {
                // Use mkvpropedit to make any changes that could not be done through makemkvcon:
                //   1/ Disable tracks which could not be deselected using selection rules:
                //          Enable flag = false
                //   2/ Update title and track properties:
                //          Title/name
                //          Language
                //          Default flag

                MKVFile mkvFile = new MKVFile(mkvToolNix);

                Debug.WriteLine("Title: " + this.Name);
                
                if (mkvFile.Open(tmpFolder + "\\" + tempFilename) == MKVFile.OperationResult.FileOpened)
                {
                    // Apply corrections to mkvFile.Title
                    mkvFile.Title = this.Name;
                    bool tracksMatchExpected = tracksAsExpected(tracksIncluded, mkvFile.Tracks);

                    // Check if there is a match between:
                    //      1/ tracksIncluded (the tracks expected to be included)
                    //      2/ mkvFile.Tracks (the actual tracks included)
                    if (tracksMatchExpected)
                    {
                        // Pass Tracks to delegate for any further modifications before writing the changes.
                        if (this.MakeMKV.ModifyTrackSettingsAfterConversion != null)
                            this.MakeMKV.ModifyTrackSettingsAfterConversion(tracksIncluded);

                        // Apply corrections to mkvFile.Tracks
                        int v = 0;
                        for(int t = 0; t < tracksIncluded.Count; t++)
                        {
                            if (!tracksIncluded[t].IsEmpty)
                            {
                                mkvFile.Tracks[v].Name = tracksIncluded[t].Name;
                                mkvFile.Tracks[v].Enabled = tracksIncluded[t].Include;
                                if (tracksIncluded[t].Language != Language.Undetermined) mkvFile.Tracks[v].Language = tracksIncluded[t].Language;
                                mkvFile.Tracks[v].Default = tracksIncluded[t].Default;
                                v++;
                            }
                        }
                    }
                    else
                    {
                        if (makeMKV.EventLogger != null)
                        {
                            StringBuilder trackInfo = new StringBuilder();
                            trackInfo.AppendLine("Tracks in output file does not match expected tracks.");
                            trackInfo.AppendLine("");
                            trackInfo.AppendLine(string.Format("File: {0}", tmpFolder + "\\" + tempFilename));
                            trackInfo.AppendLine("Tracks expected:");
                            foreach (Track track in tracksIncluded)
                                trackInfo.AppendLine(string.Format("  Type: {0}, Name: {1}, Language: {2}, Codec: {3}, Forced: {4}, IsEmpty: {5}", track.TrackType.ToString(), track.Name, track.Language, (track.TrackType == TrackType.Audio ? track.AudioCodec.ToString() : (track.TrackType == TrackType.Subtitle ? track.SubtitleCodec.ToString() : track.VideoCodec.ToString())), track.SubtitleForced, track.IsEmpty));
                            trackInfo.AppendLine(string.Format("Tracks empty (MakeMKV numbering): {0}", string.Join(", ", tracksIgnored)));
                            trackInfo.AppendLine("Tracks actual:");
                            foreach (MKVFile.Track track in mkvFile.Tracks)
                                trackInfo.AppendLine(string.Format("  Type: {0}, Name: {1}, Language: {2}, Codec: {3}, Forced: {4}", track.Type.ToString(), track.Name, track.Language, track.CodecID, track.Forced));
                            trackInfo.AppendLine(string.Format("Tracks actual match expected: " + tracksMatchExpected));

                            makeMKV.EventLogger.LogEntry("MakeMKV", trackInfo.ToString(), EventLogEntryType.Error);
                        }

                        OnConversionFailedEvent(new ConversionEventArgs("Tracks in output file does not match expected tracks.", ConversionResult.TracksNotAsExpected)); return;
                    }

                    MKVFile.OperationResult mkvEditResult = mkvFile.Save();
                    if (mkvEditResult == MKVFile.OperationResult.FileSaved || mkvEditResult == MKVFile.OperationResult.NoChanges)
                        mkvFileCorrected = true;
                }
                else
                {
                    OnConversionFailedEvent(new ConversionEventArgs("Could not modify metadata in output file.", ConversionResult.CouldNotModifyMetadata)); return;
                }
            }

            lock (conversionLock) { if (cancelRequested) { cleanup(profile, tmpFolder); OnConversionFailedEvent(new ConversionEventArgs("Cancelled.", ConversionResult.CancelledByUser)); return; } }

            // If output file exists and overwrite is allowed, attempt to delete output file.
            string outputTarget = this.OutputFullName;
            if (makeMKV.AllowOutputFileOverwrite && File.Exists(outputTarget))
            {
                try { File.Delete(outputTarget); }
                catch { }  
            }

            // NOTE: Filename suffix (if necessary) will not be added to OutputFilename.
            // Check if file already exists (because overwrite was not allowed, or deletion of existing file failed).
            // If file exists, append ' (n)' to filename, n being the first available 1-based integer where a file does not exist.
            int n = 0;
            outputTarget = OutputFullNameBase;
            while (n < 999 && File.Exists(outputTarget + (n > 0 ? String.Format(" ({0})", n) : "") + ".mkv"))
                n++;
            string outputTargetSuffix = (n > 0 ? String.Format(" ({0})", n) : "") + ".mkv";
            
            // Finally verify that target file does not exist (in case even n = 999 is used).
            if (File.Exists(outputTarget + outputTargetSuffix))
            { cleanup(null, tmpFolder); OnConversionFailedEvent(new ConversionEventArgs("Could not move output file to destination: " + outputTarget + outputTargetSuffix, ConversionResult.CouldNotMoveOutputFileToDestination)); return; }

            // Move and rename (if necessary) file to OutputFolder/OutputFilename
            try { File.Move(tmpFolder + "\\" + tempFilename, outputTarget + outputTargetSuffix); }
            catch { cleanup(null, tmpFolder); OnConversionFailedEvent(new ConversionEventArgs("Could not move output file to destination: " + outputTarget + outputTargetSuffix, ConversionResult.CouldNotMoveOutputFileToDestination)); return; }

            // Remove temp folder if it is empty
            try { Directory.Delete(tmpFolder); }
            catch { /* fail silently - not a critical error */ }

            // Get output file size
            try
            {
                FileInfo fi = new FileInfo(outputTarget + outputTargetSuffix);
                this.outputSize = fi.Length;
            }
            catch
            {
                this.outputSize = 0;
            }

            outputFilename = OutputFilenameBase + outputTargetSuffix;
            outputFilenameTemplate = null;
            TimeSpan duration = DateTime.Now - conversionStarted;
            OnConversionCompletedEvent(new ConversionEventArgs("Copy completed in " + duration.ToString(@"hh\:mm\:ss") + "." + (mkvToolNix != null && !mkvFileCorrected ? " Adjustment of file properties using MKVToolNix failed." : ""), ConversionResult.Success));
        }

        private void cleanup(string profile, string tmpFolder)
        {
            if (profile != null)
            {
                // Delete conversion profile
                try { File.Delete(profile); }
                catch { }
            }

            if (tmpFolder != null)
            {
                // Remove temp folder if it is empty
                try { Directory.Delete(tmpFolder); }
                catch { }
            }
        }

        void ReadStdOut()
        {
            string line;
            while ((line = process.StandardOutput.ReadLine()) != null)
            {
                Console.WriteLine(line);

                string value;
                Match match = Regex.Match(line, "^([A-Z]{3,5}):([0-9]+),([0-9]+),(.*)$");
                if (match.Success)
                {
                    switch (match.Groups[1].Value)
                    {
                        case "PRGC":
                        case "PRGT":
                            value = match.Groups[4].Value;
                            value = value.Substring(1, value.Length - 2).Trim();
                            lock (conversionEventArgs) { if (match.Groups[1].Value == "PRGC") { conversionEventArgs.CurrentProgressTitle = value; } else { conversionEventArgs.TotalProgressTitle = value; } }
                            OnConversionProgressEvent(conversionEventArgs);
                            break;
                        case "PRGV":
                            lock (conversionEventArgs)
                            {
                                conversionEventArgs.CurrentProgress = double.Parse(match.Groups[2].Value) / 65536;
                                conversionEventArgs.TotalProgress = double.Parse(match.Groups[3].Value) / 65536;
                            }
                            OnConversionProgressEvent(conversionEventArgs);
                            break;
                        case "MSG":
                            switch(int.Parse(match.Groups[2].Value))
                            {
                                case 4001:
                                    // Empty subtitle track
                                    match = Regex.Match(line, ",\"([0-9]+)\"$");
                                    if (match.Success) tracksIgnored.Add(int.Parse(match.Groups[1].Value));
                                    break;
                                case 5005:
                                    // Title saved, copy complete.
                                    lock(conversionLock)
                                        copyComplete = true;
                                    break;
                            }
                            break;
                    }
                }
            }

            lock (conversionLock)
                finishedReadingOutput = true;
        }

        public enum ConversionResult : short
        {
            [Description("Ready for conversion")]
            NotAvailable = 1,
            [Description("Conversion completed")]
            Success = 0,
            [Description("Unspecified error")]
            UnspecifiedError = -1,
            [Description("Could not create conversion profile")]
            CouldNotCreateConversionProfile = -2,
            [Description("MakeMKV executable not found")]
            MakeMKVConverterNotFound = -3,
            [Description("Could not access or create output folder")]
            CouldNotAccessOrCreateOutputFolder = -4,
            [Description("Could not access or create temporary folder")]
            CouldNotAccessOrCreateTemporaryFolder = -5,
            [Description("Conversion cancelled")]
            CancelledByUser = -6,
            [Description("Copy failed")]
            MakeMKVCopyFailed = -7,
            [Description("Output folder not specified")]
            OutputFolderNotSpecified = -8,
            [Description("Could not move output file to destination")]
            CouldNotMoveOutputFileToDestination = -9,
            [Description("Could not start MakeMKV")]
            CouldNotStartMakeMKV = -10,
            [Description("Conversion already in progress")]
            ConversionAlreadyInProgress = -11,
            [Description("Output file already exists")]
            OutputFileAlreadyExists = -12,
            [Description("Conversion in progress")]
            ConversionInProgress = -13,
            [Description("Could not modify selection string")]
            CouldNotModifySelectionString = -14,
            [Description("Drive not found")]
            DriveNotFound = -15,
            [Description("Source not found")]
            SourceNotFound = -16,
            [Description("Tracks in output file not as expected")]
            TracksNotAsExpected = -17,
            [Description("Could not modify output file metadata")]
            CouldNotModifyMetadata = -18,
            [Description("Output path or filename is invalid")]
            OutputPathOrFilenameInvalid = -19,
        }

        #endregion

        #region Conversion profile

        public bool CreateConversionProfile(string Filename, out List<Track> TracksIncluded, out string SelectionString)
        {
            // Generate conversion profile XML and save as Filename, to include only selected tracks and apply defined order weights, as well as audio output formats.
            //
            // Returns an ordered list of tracks which will be included in the output file.
            // This list may contain tracks which have Include = false, because selection rules are not fine-grained enough to deselect them.
            // The list can be used for disabling or removing the tracks afterwards.
            // After conversion, any responses about empty subtitle tracks should be interpreted and applied to this list (remove empty subtitle tracks from list).

            TracksIncluded = new List<Track>();
            SelectionString = null;

            List<Track> tracksOrdered = this.GetTracksOrdered();
            List<Track> tracksSelected;

            List<string> selections = new List<string>();
            string rule;

            for (int i = tracksOrdered.Count - 1; i >= 0; i--)
            {
                tracksSelected = this.SelectTrack(tracksOrdered[i], out rule);

                // Add selected tracks to tracksIncluded.
                for (int j = tracksSelected.Count - 1; j >= 0; j--)
                {
                    // If Tracks already contains this track, remove it at the current position, because it will be ordered higher
                    // in the list because it is now being selected/set with lighter weight.
                    if (TracksIncluded.Contains(tracksSelected[j])) TracksIncluded.Remove(tracksSelected[j]);

                    // Add this track to Tracks.
                    TracksIncluded.Insert(0, tracksSelected[j]);
                }

                // Remove previous selections which matches this new rule.
                while (selections.Contains(rule))
                    selections.Remove(rule);

                // Add selection rule
                selections.Add(rule);
            }

            StringBuilder selection = new StringBuilder();
            selection.Append("-sel:all");
            int w = selections.Count;
            foreach(string sel in selections)
            {
                selection.Append(sel.Replace("[weight]", w.ToString()));
                w--;
            }
            SelectionString = selection.ToString();

            return makeMKV.CreateConversionProfile(Filename, SelectionString);
        }

        private List<Track> tracksOrdered = null;
        public byte GetTrackOrder(Track Track)
        {
            // Returns 0 for video tracks or tracks with Include = false.
            // For audio/subtitle tracks with Include = true, returns the order (starting with 1) of the track. Seperate order numbering for audio and subtitle tracks.

            if (Track.TrackType == TrackType.Video || !Track.Include || Track.IsEmpty)
                return 0;

            if (tracksOrdered == null)
                tracksOrdered = this.GetTracksOrdered();

            //if (Track.TrackType == TrackType.Audio)
            //    Debug.WriteLine("Track: {0} {1} {2}, in TracksOrdered: {3}", Track.TrackType, Track.Name, Track.AudioCodec, tracksOrdered.Contains(Track));

            // Get position of Track.
            byte n = 0;
            foreach(Track t in tracksOrdered)
            {
                if (t.TrackType == Track.TrackType && !t.IsEmpty) n++;
                if (t == Track) 
                    return n;
            }

            return 0;
        }

        public void ResetTracksOrdered()
        {
            tracksOrdered = null;
        }

        public List<Track> GetTracksOrdered()
        {
            // Returns a list with all tracks with Include = true, in the order they 
            // should be in according to DEFAULT ordering and any ORDER WEIGHTS set.

            SortedList<double, Track> trks = new SortedList<double, Track>();
            double[] dt = new double[3] { 0, 1000, 1000000 };   // Regardless of order weights, order must always be: video -> audio -> subtitles
            double d = 0;
            foreach (Track t in this.Tracks)
            {
                foreach (Track track in new Track[] { t, t.Child })
                {
                    if (track != null && track.Include)
                        trks.Add((double)track.OrderWeight + dt[(int)track.TrackType - 1] + (d += 0.001), track);
                }
            }
            return trks.Values.ToList();
        }

        public List<Track> SelectTrack(Track Track, out string SelectionRule)
        {
            // Returns a list with all tracks matching the selection rules and language that can be used for the specified Track,
            // in DEFAULT order, ignoring any order weights set.
            //
            // Also returns a selection rule string to use for selecting the Track (and any extra tracks that cannot be selected) with MakeMKV.

            SelectionRule = "";
            List<Track> trks = new List<Track>();
            string tokens = "";

            switch(Track.TrackType)
            {
                case TrackType.Video:
                    if (Track.StreamFlags.HasFlag(StreamFlag.DerivedStream)) return trks;
                    tokens = "video";
                    if (!VideoInclude3D) tokens += "*(!mvcvideo)";
                    break;
                case TrackType.Audio:
                    tokens = "audio";
                    if (Track.AudioChannels == 1)
                        tokens += "*mono";
                    else if (Track.AudioChannels == 2)
                        tokens += "*stereo";
                    else
                        tokens += "*multi";
                    tokens += "*" + (MakeMKV.GetAudioCodecIsLossless(Track.AudioCodec) ? "lossless" : "lossy");
                    tokens += (Track.StreamFlags.HasFlag(StreamFlag.CoreAudio) ? "*core" : "*(!core)");
                    tokens += (Track.StreamFlags.HasFlag(StreamFlag.HasCoreAudio) ? "*havecore" : "*(!havecore)");
                    break;
                case TrackType.Subtitle:
                    tokens = "subtitle";
                    tokens += (Track.StreamFlags.HasFlag(StreamFlag.ForcedSubtitles) ? "*forced" : "*(!forced)");
                    break;
            }

            if (Track.Language != Language.Undetermined)
            {
                if (tokens.Length > 0) tokens += "*";
                tokens += MakeMKV.GetLanguageISOCode(Track.Language);
            }

            foreach(Track t0 in this.Tracks)
            {
                foreach(Track t in new Track[] { t0, t0.Child })
                {
                    if (t == null)
                        continue;
                    else if (t == Track || t.IsSelectionRuleMatch(Track))
                        trks.Add(t);
                }
            }

            if (tokens.Length > 0)
                SelectionRule = String.Format(",+sel:{0},=[weight]:{0}", new object[] { tokens });
            return trks;
        }

        #endregion

        #region Dynamic naming using tags

        public static string GetOutputFolderByTemplate(Source Source, string Template)
        {
            return getProcessedName(Source, Template);
        }

        private static string getProcessedName(Title title, string template)
        {
            return TemplateProcessor.GetProcessedName(title, title.Source, template);
        }

        private static string getProcessedName(Source source, string template)
        {
            return TemplateProcessor.GetProcessedName(null, source, template);
        }


        #endregion

    }

    public class Track
    {
        private readonly bool ignoreOrderWeightFromScan = true;

        public Track(List<string> ScanInfo, Title Title)
        {
            // ScanInfo is all lines from makemkvcon for this track.
            this.include = true;
            this.title = Title;
            this.orderWeight = 90;

            Regex regex = new Regex("^SINFO:[0-9]+,([0-9]+),([0-9]+),([0-9]+),\"(.*)\"$");
            Match match;

            int id, identifier;
            string value, codec = null, codecExtra = null;
            videoCodec = VideoCodec.Unknown;
            audioCodec = AudioCodec.Unknown;
            subtitleCodec = SubtitleCodec.Unknown;
            audioChannels = 0;
            streamFlags = 0;
            index = -1;

            if (ScanInfo == null || ScanInfo.Count == 0)
                return;

            foreach (string line in ScanInfo)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                match = regex.Match(line);
                if (!match.Success || match.Groups.Count < 5)
                    continue;

                if (index < 0 && !int.TryParse(match.Groups[1].Value, out index))
                    break;

                if (!int.TryParse(match.Groups[2].Value, out id))
                    continue;

                if (!int.TryParse(match.Groups[3].Value, out identifier))
                    identifier = 0;

                value = (!String.IsNullOrWhiteSpace(match.Groups[4].Value) ? match.Groups[4].Value.Trim() : "");

                switch(id)
                {
                    case 1:
                        switch(identifier)
                        {
                            case 6201: trackType = TrackType.Video; break;
                            case 6202: trackType = TrackType.Audio; break;
                            case 6203: trackType = TrackType.Subtitle; break;
                            default:
                                // Unsupported track type
                                index = -1;
                                return;
                        }
                        break;
                    case 2:
                        name = value;
                        nameDefault = value;
                        break;
                    case 3:
                        language = MakeMKV.GetLanguage(value);
                        break;
                    case 5:
                        if (trackType == TrackType.Subtitle) codec = value;
                        break;
                    case 6:
                        if (trackType != TrackType.Subtitle) codec = value;
                        break;
                    case 7:
                        if (trackType != TrackType.Subtitle) codecExtra = value;
                        break;
                    case 14:
                        if (!int.TryParse(value, out audioChannels)) audioChannels = 0;
                        break;
                    case 17:
                        if (!int.TryParse(value, out audioSampleRate)) audioSampleRate = 0;
                        break;
                    case 18:
                        if (!int.TryParse(value, out audioBitdepth)) audioBitdepth = 0;
                        break;
                    case 19:
                        videoResolution = MakeMKV.GetVideoResolution(value);
                        break;
                    case 20:
                        videoAspectRatio = MakeMKV.GetVideoAspectRatio(value);
                        break;
                    case 21:
                        videoFramerate = MakeMKV.GetVideoFramerate(value);
                        break;
                    case 22:
                        if (!int.TryParse(value, out id)) continue;
                        streamFlags = (StreamFlag)id;
                        break;
                    case 28:
                        metadataLanguage = MakeMKV.GetLanguage(value);
                        break;
                    case 33:
                        if (!ignoreOrderWeightFromScan && !int.TryParse(value, out orderWeight)) orderWeight = 90;
                        break;
                    case 38:
                        if (value.Contains('d'))
                            this.Default = true;
                        break;
                    case 40:
                        audioChannelLayout = MakeMKV.GetAudioChannelLayout(value);
                        break;
                }
            }

            if (codec != null)
            {
                switch (trackType)
                {
                    case TrackType.Video: 
                        videoCodec = MakeMKV.GetVideoCodec(codec);
                        break;
                    case TrackType.Audio: 
                        audioCodec = MakeMKV.GetAudioCodec(codec, audioChannels, streamFlags);
                        streamFlags |= MakeMKV.GetAudioObjectAudioFlag(audioCodec, nameDefault, codecExtra);
                        break;
                    case TrackType.Subtitle:
                        subtitleCodec = MakeMKV.GetSubtitleCodec(codec);
                        break;
                }
            }
        }

        #region Properties

        private Title title;
        public Title Title { get { return title; } }

        private Track parent = null;
        public Track Parent { get { return parent; } set { parent = value; } }

        private Track child = null;
        public Track Child { get { return child; } set { child = value; } }

        public byte TrackOrder
        {
            get
            {
                if (this.TrackType == TrackType.Video)
                    return 0;
                else
                    return this.Title.GetTrackOrder(this);
            }

        }

        private bool def = false;
        public bool Default
        {
            get
            {
                return def;
            }
            set
            {
                if (value == def) return;

                if (value)
                {
                    if (!this.Include) return;

                    def = true;

                    // Remove default flag from all other tracks of same type
                    foreach (Track t in title.Tracks)
                    {
                        if (t.TrackType == TrackType.Video) 
                            continue;
                        if (t != this && t.TrackType == this.TrackType)
                            t.Default = false;
                        if (t.Child != null && t.Child != this && t.Child.TrackType == this.TrackType)
                            t.Child.Default = false;
                    }
                }
                else
                {
                    if (this.TrackType != TrackType.Audio)
                    {
                        def = false;
                        return;
                    }

                    // If this track is not included, always allow setting Default = false
                    if (!this.Include)
                        def = false;

                    // Check if there are other audio tracks which are already set as Default, or which can be set as default (choose first ordered track (first / lightest)).
                    // Only set Default = false for this track if that is the case.
                    SortedList<double, Track> trks = new SortedList<double, Track>();
                    double d = 0;
                    foreach (Track t in title.Tracks)
                    {
                        if (t.TrackType != TrackType.Audio) continue;
                        foreach (Track track in new Track[] { t, t.Child })
                        {
                            if (track != null && track != this && track.Include)
                            {
                                if (track.Default)
                                {
                                    // There is already another audio track set as Default, OK!
                                    def = false;
                                    return;
                                }
                                trks.Add((double)track.OrderWeight + (d += 0.001), track);
                            }
                        }
                    }

                    if (trks.Count > 0)
                    {
                        // There is another audio track with Include = true which can be set as Default, OK!
                        def = false;
                        trks.First().Value.Default = true;
                    }
                }
            }
        }

        public bool IsEmpty { 
            get { return streamFlags.HasFlag(StreamFlag.IsEmpty); } 
            set {
                if (value && !streamFlags.HasFlag(StreamFlag.IsEmpty))
                    streamFlags |= StreamFlag.IsEmpty;
                else if (!value && streamFlags.HasFlag(StreamFlag.IsEmpty))
                    streamFlags &= ~StreamFlag.IsEmpty;
            } 
        }

        public bool IsValidChild(Track Track)
        {
            if (Track.TrackType != this.TrackType) return false;

            switch (this.TrackType)
            {
                case TrackType.Video:
                    return (Track.VideoCodec == VideoCodec.MPEG4MVC && Track.StreamFlags.HasFlag(StreamFlag.DerivedStream));
                case TrackType.Audio:
                    return (Track.StreamFlags.HasFlag(StreamFlag.CoreAudio) && this.StreamFlags.HasFlag(StreamFlag.HasCoreAudio) && Track.Language == this.Language);
                case TrackType.Subtitle:
                    return (Track.SubtitleForced && Track.SubtitleCodec == this.SubtitleCodec && Track.Language == this.Language);
                default:
                    return false;
            }
        }

        public bool IsSelectionRuleMatch(Track Track)
        {
            // Returns true, if selection rules for Track would also include this track.

            // Track type / language
            if (this.TrackType != Track.TrackType) return false;
            if (Track.Language != Language.Undetermined && this.Language != Track.Language) return false;

            switch(this.TrackType)
            {
                case TrackType.Video:
                    return (Track.Child == null && !Track.StreamFlags.HasFlag(StreamFlag.DerivedStream));
                case TrackType.Audio:
                    if ((this.AudioChannels == 1 && Track.AudioChannels > 1) ||
                        (this.AudioChannels == 2 && Track.AudioChannels != 2) ||
                        (this.AudioChannels > 2 && Track.AudioChannels <= 2))
                        return false;
                    if (MakeMKV.GetAudioCodecIsLossless(this.AudioCodec) != MakeMKV.GetAudioCodecIsLossless(Track.AudioCodec)) return false;
                    if (this.StreamFlags.HasFlag(StreamFlag.CoreAudio) != Track.StreamFlags.HasFlag(StreamFlag.CoreAudio)) return false;
                    if (this.StreamFlags.HasFlag(StreamFlag.HasCoreAudio) != Track.StreamFlags.HasFlag(StreamFlag.HasCoreAudio)) return false;
                    break;
                case TrackType.Subtitle:
                    if (this.StreamFlags.HasFlag(StreamFlag.ForcedSubtitles) != Track.StreamFlags.HasFlag(StreamFlag.ForcedSubtitles)) return false;
                    break;
            }

            return true;
        }

        private int index;
        public int Index { get { return index; } }

        private bool include;
        public bool Include { 
            get { return include; } 
            set {
                if (value == include) return;
                include = (trackType == TrackType.Video && !streamFlags.HasFlag(StreamFlag.DerivedStream)) ? true : value;
                if (trackType == TrackType.Subtitle) 
                {
                    if (this.child != null)
                        this.child.Include = value;
                    else if (this.parent != null)
                        this.parent.Include = value;
                }

                if (!include)
                {
                    // If this track is not included, it cannot be the default track
                    this.Default = false;
                }
                else if (trackType == TrackType.Audio)
                {
                    // Check if there are no default audio tracks, in that case, set this track to default
                    bool defaultTrack = false;
                    foreach (Track t in this.Title.Tracks)
                        if (t.TrackType == TrackType.Audio && t.Default)
                        { defaultTrack = true; break; }
                    if (!defaultTrack)
                        this.Default = true;
                }

                // Update track ordering
                if (trackType == TrackType.Audio || trackType == TrackType.Subtitle)
                    this.Title.ResetTracksOrdered();
            } 
        }

        private int orderWeight;
        public int OrderWeight { 
            get { return orderWeight; }
            set {
                if (value == orderWeight) return;
                orderWeight = (value > 100) ? 100 : ((value < -100) ? -100 : value);
                this.Title.ResetTracksOrdered();
            } 
        }

        private TrackType trackType;
        public TrackType TrackType { get { return trackType; } }

        private string name, nameDefault;
        public string Name { get { return name; } set { name = value; } }

        public bool NameModified { get { return (name != nameDefault); } }

        private Language metadataLanguage;
        public Language MetadataLanguage { get { return metadataLanguage; } set { metadataLanguage = value; } }

        private Language language;
        public Language Language { get { return language; } set { language = value; } }

        public AudioOutputFormat AudioOutputFormat(MakeMKV MakeMKV)
        {
            return MakeMKV.AudioOutputFormat(this.audioCodec);
        }

        private AudioCodec audioCodec;
        public AudioCodec AudioCodec { get { return audioCodec; } }

        private StreamFlag streamFlags;
        public StreamFlag StreamFlags { get { return streamFlags; } }

        private int audioChannels;
        public int AudioChannels { get { return audioChannels; } }

        public bool AudioHasObjectAudio
        {
            get { return streamFlags.HasFlag(StreamFlag.HasObjectAudio); }
        }

        public string AudioObjectAudioDescription
        {
            get { return (AudioHasObjectAudio ? MakeMKV.GetObjectAudioDescription(audioCodec) : null); }
        }

        private AudioChannelLayout audioChannelLayout;
        public AudioChannelLayout AudioChannelLayout { get { return audioChannelLayout; } }

        private int audioSampleRate;
        public int AudioSampleRate { get { return audioSampleRate; } }

        private int audioBitdepth;
        public int AudioBitdepth { get { return audioBitdepth; } }

        private VideoCodec videoCodec;
        public VideoCodec VideoCodec { get { return videoCodec; } }

        private Size videoResolution;
        public Size VideoResolution { get { return videoResolution; } }

        private double videoAspectRatio;
        public double VideoAspectRatio { get { return videoAspectRatio; } }

        private double videoFramerate;
        public double VideoFramerate { get { return videoFramerate; } }

        private SubtitleCodec subtitleCodec;
        public SubtitleCodec SubtitleCodec { get { return subtitleCodec; } }

        public bool SubtitleForced { get { return streamFlags.HasFlag(StreamFlag.ForcedSubtitles); } }

        public string MKVFlags {
            get 
            { 
                return (def ? "d" : ""); 
            } 
            set 
            {
                if (value != null)
                {
                    this.Default = value.Contains('d');
                }
            } 
        }

        #endregion

    }

    #region Event argument classes

    public class ProgressEventArgs : EventArgs
    {
        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

        private string titleName;
        public string TitleName { get { return titleName; } }

        public ProgressEventArgs(string TitleName = null)
        {
            this.titleName = TitleName;
        }

        private string currentProgressTitle = "";
        public string CurrentProgressTitle { get { return currentProgressTitle; } set { currentProgressTitle = value; } }

        private double currentProgress = 0;
        public double CurrentProgress { get { return currentProgress; } set { currentProgress = value; } }

        private string totalProgressTitle = "";
        public string TotalProgressTitle { get { return totalProgressTitle; } set { totalProgressTitle = value; } }

        private double totalProgress = 0;
        public double TotalProgress { get { return totalProgress; } set { totalProgress = value; } }
    }

    public class ScanEventArgs : EventArgs
    {
        public delegate void ScanEventHandler(object sender, ScanEventArgs e);

        public ScanEventArgs(string Message, Source.ScanResult ScanResult = Source.ScanResult.NotAvailable)
        {
            this.message = Message;
            this.scanResult = ScanResult;
        }

        private string message;
        public string Message { get { return message; } }

        private Source.ScanResult scanResult;
        public Source.ScanResult ScanResult { get { return scanResult; } }

        // Applies only for OnStarted events.
        private bool cancel = false;
        public bool Cancel { get { return cancel; } set { cancel = value; } }
    }

    public class ConversionEventArgs : EventArgs
    {
        public delegate void ConversionEventHandler(object sender, ConversionEventArgs e);

        public ConversionEventArgs(string Message, Title.ConversionResult ConversionResult = Title.ConversionResult.NotAvailable)
        {
            this.message = Message;
            this.conversionResult = ConversionResult;
        }

        private string message;
        public string Message { get { return message; } }

        private Title.ConversionResult conversionResult;
        public Title.ConversionResult ConversionResult { get { return conversionResult; } }

        // Applies only for OnStarted events.
        private bool cancel = false;
        public bool Cancel { get { return cancel; } set { cancel = value; } }
    }

    #endregion

    #region Enums

    public enum AudioChannelLayout
    {
        Unknown = 0,
        Mono = 1,
        Stereo = 2,
        [Description("Surround 5.1")]
        Surround_51_Side = 3,
        [Description("Surround 5.1")]
        Surround_51_Rear = 4,
        [Description("Surround 7.1")]
        Surround_71 = 5
    }

    public enum VideoCodec : byte
    {
        Unknown = 0,
        MPEG1 = 1,
        MPEG2 = 2,
        MPEG4 = 3,
        VC1 = 4,
        [Description("MPEG4-MVC-3D")]
        MPEG4MVC = 5,
        [Description("MPEGH/HVC")]
        MPEGHHVC = 6

    }

    public enum SubtitleCodec : byte       // note: 'forced' is a separate flag
    {
        Unknown = 0,
        VOBSUB = 1,
        VOBSUBHD = 3,
        PGS = 5
    }

    public enum AudioCodec : byte
    {
        Unknown = 0,
        
        MP2 = 1,
        MP3 = 2,

        [Description("Dolby Digital")]
        AC3_Stereo = 3,
        [Description("Dolby Digital")]
        AC3_Multi = 4,

        [Description("DTS")]
        DTS_Stereo = 5,
        [Description("DTS")]
        DTS_Multi = 6,

        [Description("Dolby Digital Plus")]
        EAC3_Stereo = 7,
        [Description("Dolby Digital Plus")]
        EAC3_Multi = 8,

        [Description("MLP")]
        MLP_Stereo = 9,
        [Description("MLP")]
        MLP_Multi = 10,
        
        [Description("Dolby TrueHD")]
        TrueHD_Stereo = 11,
        [Description("Dolby TrueHD")]
        TrueHD_Multi = 12,

        [Description("Dolby Digital")]
        TrueHD_Core_Stereo = 13,
        [Description("Dolby Digital")]
        TrueHD_Core_Multi = 14,
        
        [Description("DTS-HD MA")]
        DTSHDMasterAudio_Stereo = 15,
        [Description("DTS-HD MA")]
        DTSHDMasterAudio_Multi = 16,

        [Description("DTS-HD LBR")]
        DTSHDLBR_Stereo = 17,
        [Description("DTS-HD LBR")]
        DTSHDLBR_Multi = 18,

        [Description("DTS-HD HR")]
        DTSHD_Stereo = 19,
        [Description("DTS-HD HR")]
        DTSHD_Multi = 20,

        [Description("DTS")]
        DTSHD_Core_Stereo = 21,
        [Description("DTS")]
        DTSHD_Core_Multi = 22,

        [Description("LPCM")]
        LPCM_Stereo = 23,
        [Description("LPCM")]
        LPCM_Multi = 24,
        
        [Description("FLAC")]
        FLAC_Stereo = 25,
        [Description("FLAC")]
        FLAC_Multi = 26,        
    }

    public enum AudioOutputFormat
    {
        [Description("Direct copy")]
        DirectCopy = 0,
        FLAC = 1,
        LPCM = 2,
    }

    public enum FLACCompression
    {
        Fast = 0,
        Good = 1,
        Best = 2
    }

    public enum SubtitleCompression
    {
        None = 0,
        zlib = 1
    }

    public enum LPCMContainer
    {
        Raw = 0,
        Wavex = 1
    }

    public enum TrackType
    {
        Video = 1,
        Audio = 2,
        Subtitle = 3
    }

    public enum DiscType : byte
    {
        DVD = 1,
        Bluray = 2,
        File = 3
    }

    public enum StreamFlag
    {
        [Description("Commentary")]
        Commentary = 1,
        [Description("Alternative commentary")]
        AlternativeCommentary = 2,
        [Description("For visually impaired")]
        ForVisuallyImpaired = 4,
        [Description("Core audio")]
        CoreAudio = 256,
        [Description("Secondary audio")]
        SecondaryAudio = 512,
        [Description("Has core audio")]
        HasCoreAudio = 1024,
        [Description("Is derived stream")]
        DerivedStream = 2048,
        [Description("Forced subtitles")]
        ForcedSubtitles = 4096,
        [Description("Secondary stream")]
        ProfileSecondaryStream = 16384,
        [Description("Empty track")]
        IsEmpty = 32768,
        [Description("Has object audio metadata")]
        HasObjectAudio = 65536,
    }

    public enum Language : short
    {
        Undetermined = 0,
        Abkhazian = 1,
        Achinese = 2,
        Acoli = 3,
        Adangme = 4,
        Adyghe = 5,
        Afar = 6,
        Afrihili = 7,
        Afrikaans = 8,
        Ainu = 9,
        Akan = 10,
        Albanian = 11,
        Aleut = 12,
        Amharic = 13,
        Angika = 14,
        Arabic = 15,
        Aragonese = 16,
        Arapaho = 17,
        Arawak = 18,
        Armenian = 19,
        Assamese = 20,
        Asturian = 21,
        Avaric = 22,
        Awadhi = 23,
        Aymara = 24,
        Azerbaijani = 25,
        Balinese = 26,
        Baluchi = 27,
        Bambara = 28,
        Basa = 29,
        Bashkir = 30,
        Basque = 31,
        Beja = 32,
        Belarusian = 33,
        Bemba = 34,
        Bengali = 35,
        Bhojpuri = 36,
        Bikol = 37,
        Bilin = 38,
        Bini = 39,
        Bislama = 40,
        Bosnian = 41,
        Braj = 42,
        Breton = 43,
        Buginese = 44,
        Bulgarian = 45,
        Buriat = 46,
        Burmese = 47,
        Caddo = 48,
        Catalan = 49,
        Cebuano = 50,
        [Description("Central Khmer")]
        CentralKhmer = 51,
        Chamorro = 52,
        Chechen = 53,
        Cherokee = 54,
        Cheyenne = 55,
        Chinese = 56,
        [Description("Chinook jargon")]
        Chinookjargon = 57,
        Chipewyan = 58,
        Choctaw = 59,
        Chuukese = 60,
        Chuvash = 61,
        Cornish = 62,
        Corsican = 63,
        Cree = 64,
        Creek = 65,
        [Description("Crimean Tatar")]
        CrimeanTatar = 66,
        Croatian = 67,
        Czech = 68,
        Dakota = 69,
        Danish = 70,
        Dargwa = 71,
        Delaware = 72,
        Dhivehi = 73,
        Dinka = 74,
        Dogri = 75,
        Dogrib = 76,
        Duala = 77,
        Dutch = 78,
        Dyula = 79,
        Dzongkha = 80,
        [Description("Eastern Frisian")]
        EasternFrisian = 81,
        Efik = 82,
        Ekajuk = 83,
        English = 84,
        Erzya = 85,
        Esperanto = 86,
        Estonian = 87,
        Ewe = 88,
        Ewondo = 89,
        Fang = 90,
        Fanti = 91,
        Faroese = 92,
        Fijian = 93,
        Filipino = 94,
        Finnish = 95,
        Fon = 96,
        French = 97,
        Friulian = 98,
        Fulah = 99,
        Ga = 100,
        [Description("Galibi Carib")]
        GalibiCarib = 101,
        Galician = 102,
        Ganda = 103,
        Gayo = 104,
        Gbaya = 105,
        Georgian = 106,
        German = 107,
        Gilbertese = 108,
        Gondi = 109,
        Gorontalo = 110,
        Grebo = 111,
        Guarani = 112,
        Gujarati = 113,
        Gwichin = 114,
        Haida = 115,
        Haitian = 116,
        Hausa = 117,
        Hawaiian = 118,
        Hebrew = 119,
        Herero = 120,
        Hiligaynon = 121,
        Hindi = 122,
        [Description("Hiri Motu")]
        HiriMotu = 123,
        Hmong = 124,
        Hungarian = 125,
        Hupa = 126,
        Iban = 127,
        Icelandic = 128,
        Ido = 129,
        Igbo = 130,
        Iloko = 131,
        [Description("Inari Sami")]
        InariSami = 132,
        Indonesian = 133,
        Ingush = 134,
        Inuktitut = 135,
        Inupiaq = 136,
        Irish = 137,
        Italian = 138,
        Japanese = 139,
        Javanese = 140,
        [Description("Judeo - Arabic")]
        JudeoArabic = 141,
        [Description("Judeo - Persian")]
        JudeoPersian = 142,
        Kabardian = 143,
        Kabyle = 144,
        Kachin = 145,
        Kalmyk = 146,
        Kalaallisut = 147,
        Kamba = 148,
        Kannada = 149,
        Kanuri = 150,
        [Description("Karachay - Balkar")]
        KarachayBalkar = 151,
        [Description("Kara - Kalpak")]
        KaraKalpak = 152,
        Karelian = 153,
        Kashmiri = 154,
        Kashubian = 155,
        Kazakh = 156,
        Khasi = 157,
        Kikuyu = 158,
        Kimbundu = 159,
        Kinyarwanda = 160,
        Kirghiz = 161,
        Klingon = 162,
        Komi = 163,
        Kongo = 164,
        Konkani = 165,
        Korean = 166,
        Kosraean = 167,
        Kpelle = 168,
        Kuanyama = 169,
        Kumyk = 170,
        Kurdish = 171,
        Kurukh = 172,
        Kutenai = 173,
        Ladino = 174,
        Lahnda = 175,
        Lamba = 176,
        Lao = 177,
        Latin = 178,
        Latvian = 179,
        Lezghian = 180,
        Limburgan = 181,
        Lingala = 182,
        Lithuanian = 183,
        Lojban = 184,
        [Description("Lower Sorbian")]
        LowerSorbian = 185,
        [Description("Low German")]
        LowGerman = 186,
        Lozi = 187,
        [Description("Luba - Katanga")]
        LubaKatanga = 188,
        [Description("Luba - Lulua")]
        LubaLulua = 189,
        Luiseno = 190,
        [Description("Lule Sami")]
        LuleSami = 191,
        Lunda = 192,
        Luo = 193,
        Lushai = 194,
        Luxembourgish = 195,
        Macedonian = 196,
        [Description("Macedo - Romanian")]
        MacedoRomanian = 197,
        Madurese = 198,
        Magahi = 199,
        Maithili = 200,
        Makasar = 201,
        Malagasy = 202,
        Malay = 203,
        Malayalam = 204,
        Maltese = 205,
        Manchu = 206,
        Mandar = 207,
        Mandingo = 208,
        Manipuri = 209,
        Manx = 210,
        Maori = 211,
        Mapudungun = 212,
        Marathi = 213,
        Mari = 214,
        Marshallese = 215,
        Marwari = 216,
        Masai = 217,
        Mende = 218,
        [Description("Mi'kmaq")]
        Mikmaq = 219,
        Minangkabau = 220,
        Mirandese = 221,
        Greek = 222,
        Mohawk = 223,
        Moksha = 224,
        Mongo = 225,
        Mongolian = 226,
        Mossi = 227,
        [Description("Multiple languages")]
        Multiplelanguages = 228,
        Nauru = 229,
        Navajo = 230,
        Ndonga = 231,
        Neapolitan = 232,
        [Description("Nepal Bhasa")]
        NepalBhasa = 233,
        Nepali = 234,
        Nias = 235,
        Niuean = 236,
        [Description("N'Ko")]
        NKo = 237,
        Nogai = 238,
        [Description("No linguistic content")]
        Nolinguisticcontent = 239,
        [Description("Northern Frisian")]
        NorthernFrisian = 240,
        [Description("Northern Sami")]
        NorthernSami = 241,
        [Description("North Ndebele")]
        NorthNdebele = 242,
        Norwegian = 243,
        [Description("Norwegian Bokmål")]
        NorwegianBokmål = 244,
        [Description("Norwegian Nynorsk")]
        NorwegianNynorsk = 245,
        Nyamwezi = 246,
        Nyanja = 247,
        Nyankole = 248,
        Nyoro = 249,
        Nzima = 250,
        Occitan = 251,
        Odiya = 252,
        Ojibwa = 253,
        Oromo = 254,
        Osage = 255,
        Ossetian = 256,
        Pahlavi = 257,
        Palauan = 258,
        Pampanga = 259,
        Pangasinan = 260,
        Panjabi = 261,
        Papiamento = 262,
        Pedi = 263,
        Persian = 264,
        Pohnpeian = 265,
        Polish = 266,
        Portuguese = 267,
        Pushto = 268,
        Quechua = 269,
        Rajasthani = 270,
        Rapanui = 271,
        Rarotongan = 272,
        Romanian = 273,
        Romansh = 274,
        Romany = 275,
        Rundi = 276,
        Russian = 277,
        Samoan = 278,
        Sandawe = 279,
        Sango = 280,
        Sanskrit = 281,
        Santali = 282,
        Sardinian = 283,
        Sasak = 284,
        Scots = 285,
        [Description("Scottish Gaelic")]
        ScottishGaelic = 286,
        Selkup = 287,
        Serbian = 288,
        Serer = 289,
        Shan = 290,
        Shona = 291,
        [Description("Sichuan Yi")]
        SichuanYi = 292,
        Sicilian = 293,
        Sidamo = 294,
        Siksika = 295,
        Sindhi = 296,
        Sinhala = 297,
        [Description("Skolt Sami")]
        SkoltSami = 298,
        Slave = 299,
        Slovak = 300,
        Slovenian = 301,
        Sogdian = 302,
        Somali = 303,
        Soninke = 304,
        [Description("Southern Altai")]
        SouthernAltai = 305,
        [Description("Southern Sami")]
        SouthernSami = 306,
        [Description("Southern Sotho")]
        SouthernSotho = 307,
        [Description("South Ndebele")]
        SouthNdebele = 308,
        Spanish = 309,
        [Description("Sranan Tongo")]
        SrananTongo = 310,
        [Description("Standard Moroccan Tamazight")]
        StandardMoroccanTamazight = 311,
        Sukuma = 312,
        Sumerian = 313,
        Sundanese = 314,
        Susu = 315,
        Swahili = 316,
        Swati = 317,
        Swedish = 318,
        [Description("Swiss German")]
        SwissGerman = 319,
        Syriac = 320,
        Tagalog = 321,
        Tahitian = 322,
        Tajik = 323,
        Tamashek = 324,
        Tamil = 325,
        Tatar = 326,
        Telugu = 327,
        Tereno = 328,
        Tetum = 329,
        Thai = 330,
        Tibetan = 331,
        Tigre = 332,
        Tigrinya = 333,
        Timne = 334,
        Tiv = 335,
        Tlingit = 336,
        Tokelau = 337,
        [Description("Tok Pisin")]
        TokPisin = 338,
        [Description("Tonga(Nyasa)")]
        TongaNyasa = 339,
        [Description("Tonga(Tonga Islands)")]
        TongaTongaIslands = 340,
        Tsimshian = 341,
        Tsonga = 342,
        Tswana = 343,
        Tumbuka = 344,
        Turkish = 345,
        Turkmen = 346,
        Tuvalu = 347,
        Tuvinian = 348,
        Twi = 349,
        Udmurt = 350,
        Ugaritic = 351,
        Uighur = 352,
        Ukrainian = 353,
        Umbundu = 354,
        [Description("Uncoded languages")]
        Uncodedlanguages = 355,
        [Description("Upper Sorbian")]
        UpperSorbian = 356,
        Urdu = 357,
        Uzbek = 358,
        Vai = 359,
        Venda = 360,
        Vietnamese = 361,
        Votic = 362,
        Walloon = 363,
        Waray = 364,
        Washo = 365,
        Welsh = 366,
        [Description("Western Frisian")]
        WesternFrisian = 367,
        Wolaytta = 368,
        Wolof = 369,
        Xhosa = 370,
        Yakut = 371,
        Yao = 372,
        Yapese = 373,
        Yiddish = 374,
        Yoruba = 375,
        Zapotec = 376,
        Zaza = 377,
        Zenaga = 378,
        Zhuang = 379,
        Zulu = 380,
        Zuni = 381,

    }

    #endregion
}
