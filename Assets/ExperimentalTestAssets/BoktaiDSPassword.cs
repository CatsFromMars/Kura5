using System;
using UnityEngine;

	public class BoktaiDSPassword {
		private static int PASS_LEN     = 40;
		private static int CHAR_WIDTH   =  6;
		private static int NAME_LEN     = 10;
		private static string BASE64_EN = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890?=";
		private static string BASE64_JP = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをがぎぐげござじずぜぞだぢづでどばびぶべぼ";
		#region Character tables
		private static string TABLE
			= "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\u0020\u0021\uFFFD\uFFFD\u00F7\uFFFD\u0026\u0027\u0028\u0029\u002A\u002B\u002C\u002D\u002E\u002F"
			+ "\u0030\u0031\u0032\u0033\u0034\u0035\u0036\u0037\u0038\u0039\uFFFD\u003B\uFFFD\u003D\uFFFD\u003F"
			+ "\uFFFD\u0041\u0042\u0043\u0044\u0045\u0046\u0047\u0048\u0049\u004A\u004B\u004C\u004D\u004E\u004F"
			+ "\u0050\u0051\u0052\u0053\u0054\u0055\u0056\u0057\u0058\u0059\u005A\u005B\u00D7\u005D\uFFFD\uFFFD"
			+ "\uFFFD\u0061\u0062\u0063\u0064\u0065\u0066\u0067\u0068\u0069\u006A\u006B\u006C\u006D\u006E\u006F"
			+ "\u0070\u0071\u0072\u0073\u0074\u0075\u0076\u0077\u0078\u0079\u007A\uFFFD\uFFFD\uFFFD\uFFFD\u00B7"
			+ "\uFFFD\u00C4\uFFFD\u00C7\u00C9\u00D1\u00D6\u00DC\u00E1\u00E0\u00E2\u00E4\uFFFD\uFFFD\u00E7\u00E9"
			+ "\u00E8\u00EA\u00EB\u00ED\u00EC\u00EE\u00EF\u00F1\u00F3\u00F2\u00F4\u00F6\uFFFD\u00FA\u00F9\u00FB"
			+ "\u00FC\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\u00DF\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\u00BF\u00A1\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\u00C0\uFFFD\uFFFD\u0152"
			+ "\u0153\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\u00C2\u00CA\u00C1\u00CB\u00C8\u00CD\u00CE\u00CF\u00CC\u00D3"
			+ "\u00D4\uFFFD\u00D2\u00DA\u00DB\u00D9\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD";
		private static string TABLE_80
			= "\uFFFD\u3042\u3044\u3046\u3048\u304A\u304B\u304D\u304F\u3051\u3053\u3055\u3057\u3059\u305B\u305D"
			+ "\uFFFD\u305F\u3061\u3064\u3066\u3068\u306A\u306B\u306C\u306D\u306E\u306F\u3072\u3075\u3078\u307B"
			+ "\uFFFD\u307E\u307F\u3080\u3081\u3082\u3084\u3086\u3088\u3089\u308A\u308B\u308C\u308D\u308F\u3092"
			+ "\uFFFD\u3093\u3041\u3043\u3045\u3047\u3049\u3063\u3083\u3085\u3087\u304C\u304E\u3050\u3052\u3054"
			+ "\uFFFD\u3056\u3058\u305A\u305C\u305E\u3060\u3062\u3065\u3067\u3069\u3070\u3073\u3076\u3079\u307C"
			+ "\uFFFD\u3071\u3074\u3077\u307A\u307D\uFF61\uFF64\uFF5E\u30FC\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\u30A2\u30A4\u30A6\u30A8\u30AA\u30AB\u30AD\u30AF\u30B1\u30B3\u30B5\u30B7\u30B9\u30BB\u30BD"
			+ "\uFFFD\u30BF\u30C1\u30C4\u30C6\u30C8\u30CA\u30CB\u30CC\u30CD\u30CE\u30CF\u30D2\u30D5\u30D8\u30DB"
			+ "\uFFFD\u30DE\u30DF\u30E0\u30E1\u30E2\u30E9\u30EA\u30EB\u30EC\u30ED\u30E4\u30E6\u30E8\u30EF\u30F2"
			+ "\uFFFD\u30F3\u30A1\u30A3\u30A5\u30A7\u30A9\u30C3\u30E3\u30E5\u30E7\u30AC\u30AE\u30B0\u30B2\u30B4"
			+ "\uFFFD\u30B6\u30B8\u30BA\u30BC\u30BE\u30C0\u30C2\u30C5\u30C7\u30C9\u30D0\u30D3\u30D6\u30D9\u30DC"
			+ "\uFFFD\u30D1\u30D4\u30D7\u30DA\u30DD\u30FB\uFFFD\uFF1B\uFFFD\uFFFD\uFF0B\u00D7\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFF1D\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD";
		private static string TABLE_86
			= "\uFFFD\uFF21\uFF22\uFF23\uFF24\uFF25\uFF26\uFF27\uFF28\uFF29\uFF2A\uFF2B\uFF2C\uFF2D\uFF2E\uFF2F"
			+ "\uFF30\uFF31\uFF32\uFF33\uFF34\uFF35\uFF36\uFF37\uFF38\uFF39\uFF3A\uFF3B\uFF3D\uFF08\uFF09\uFF0A"
			+ "\uFF06\uFF41\uFF42\uFF43\uFF44\uFF45\uFF46\uFF47\uFF48\uFF49\uFF4A\uFF4B\uFF4C\uFF4D\uFF4E\uFF4F"
			+ "\uFF50\uFF51\uFF52\uFF53\uFF54\uFF55\uFF56\uFF57\uFF58\uFF59\uFF5A\uFF01\uFF1F\uFF0E\uFF0C\uFF07"
			+ "\uFF0F\uFF10\uFF11\uFF12\uFF13\uFF14\uFF15\uFF16\uFF17\uFF18\uFF19\u00F7\uFF0D\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD"
			+ "\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\uFFFD\u3000";
		#endregion

		protected enum CharacterSet {
			Unknown,
			Japanese,
			Latin
		}

		/// <summary>
		/// Boktai DS titles.
		/// </summary>
		[Flags]
		public enum Titles {
			DarkKnight		= 0x0001,
			SolGunner		= 0x0002,
			Swordmaster		= 0x0004,
			Gunmaster		= 0x0008,
			Guardian		= 0x0010,
			TreasureHunter	= 0x0020,
			Collector		= 0x0040,
			Huntmaster		= 0x0080,
			ShootingStar	= 0x0100,
			Gladiator		= 0x0200,
			SpecialAgent	= 0x0400,
			Wanderer		= 0x0800,
			Adventurer		= 0x1000,
			GrandMaster		= 0x2000
		}

		/// <summary>
		/// A Boktai DS game region.
		/// </summary>
		public enum GameRegion {
			Invalid = 0,
			Japan = 1,
			NorthAmerica = 2,
			Europe = 3
		}
		public enum GameDifficulty {
			Invalid = -1,
			Normal = 0,
			Hard = 1,
			Nightmare = 2,
		}
		/// <summary>
		/// A Boktai DS sword.
		/// </summary>
		public enum Sword {
			Invalid = -1,
			Vanargand = 0,
			Jormungandr = 1,
			Hel = 2
		}
		/// <summary>
		/// A Boktai DS gun.
		/// </summary>
		public enum Gun {
			Invalid = -1,
			Knight = 0,
			Dragoon = 1,
			Bomber = 2,
			Witch = 3,
			Ninja = 4
		}
		/// <summary>
		/// A Boktai DS Terrennial.
		/// </summary>
		public enum Terrennial {
			Invalid = -1,
			Otenko = 0,
			Nero = 1,
			Ursula = 2,
			Ezra = 3,
			Alexander = 4,
			Tove = 5,
			OmegaXis = 6
		}
		/// <summary>
		/// A Boktai DS climate.
		/// </summary>
		public enum Climate {
			Invalid = -1,
			BalmySubtropical = 0,
			AridDesert = 1,
			TropicalRainforest = 2,
			HumidContinental = 3,
			FrigidArctic = 4
		}

		private byte[] Bytes;
		
		/// <summary>
		/// Gets the game region.
		/// </summary>
		public GameRegion Region {
			get {
				return (GameRegion)ReadBits(18, 3);
			}
		}
		/// <summary>
		/// Gets the earned titles.
		/// </summary>
		public Titles EarnedTitles {
			get {
				return (Titles)ReadBits(24, 14);
			}
		}
		/// <summary>
		/// Gets the game difficulty.
		/// </summary>
		public GameDifficulty Difficulty {
			get {
				int difficulty = ReadBits(38, 2);
				if (difficulty > 2) {
					return GameDifficulty.Invalid;
				} else {
					return (GameDifficulty)difficulty;
				}
			}
		}
		/// <summary>
		/// Gets the number of hours played, rounded down to the nearest integer.
		/// </summary>
		public int HoursPlayed {
			get {
				return ReadBits(40, 7);
			}
		}
		/// <summary>
		/// Gets the amount of Soll acquired, rounded down to the nearest multiple of 4096.
		/// </summary>
		public int SollAcquired {
			get {
				return ReadBits(47, 15) * 4096;
			}
		}
		/// <summary>
		/// Gets the name of Sabata/Lucian.
		/// </summary>
		public string SabataName {
			get {
				return ReadString(74, NAME_LEN);
			}
		}
		/// <summary>
		/// Gets the name of Django/Aaron.
		/// </summary>
		public string DjangoName {
			get {
				return ReadString(154, NAME_LEN);
			}
		}
		/// <summary>
		/// Gets the favorite sword.
		/// </summary>
		public Sword FavoriteSword {
			get {
				int sword = ReadBits(62, 3);
				if (sword > 2) {
					return Sword.Invalid;
				} else {
					return (Sword)sword;
				}
			}
		}
		/// <summary>
		/// Gets the favorite gun.
		/// </summary>
		public Gun FavoriteGun {
			get {
				int gun = ReadBits(65, 3);
				if (gun > 5) {
					return Gun.Invalid;
				} else {
					return (Gun)gun;
				}
			}
		}
		/// <summary>
		/// Gets the favorite Terrennial.
		/// </summary>
		public Terrennial FavoriteTerrennial {
			get {
				int terrennial = ReadBits(68, 3);
				if (terrennial > 6) {
					return Terrennial.Invalid;
				} else {
					return (Terrennial)terrennial;
				}
			}
		}
		/// <summary>
		/// Gets the favorite climate.
		/// </summary>
		public Climate FavoriteClimate {
			get {
				int climate = ReadBits(71, 3);
				if (climate > 4) {
					return Climate.Invalid;
				} else {
					return (Climate)climate;
				}
			}
		}

		private BoktaiDSPassword(byte[] bytes) {
			this.Bytes = bytes;
		}

		/// <summary>
		/// Reads the password data from the specified password.
		/// </summary>
		/// <param name="password">The password to read.</param>
		/// <param name="data">The output password data.</param>
		/// <returns>true if reading succeeded; otherwise, false.</returns>
		public static bool Load(string password, out BoktaiDSPassword data) {
			// Init password data to null.
			data = null;

			// Fail if password string is null.
			if (password == null) {
				//Console.Error.WriteLine("Password is null.");
				return false;
			}

			// Remove spaces.
			password = password.Replace(" ", "");

			// Fail if password is not exactly PASSWORD_LENGTH characters long.
			if (password.Length != PASS_LEN) {
				//Console.Error.WriteLine("Password length error.");
				return false;
			}

			// Base64 decode.
			byte[] bytes = new byte[PASS_LEN];
			CharacterSet lang = CharacterSet.Unknown;
			for (int i = 0; i < PASS_LEN; i++) {
				int c;
				switch (lang) {
					case CharacterSet.Latin:
						c = BASE64_EN.IndexOf(password[i]);
						break;
					case CharacterSet.Japanese:
						c = BASE64_JP.IndexOf(password[i]);
						break;
					default:
						c = BASE64_EN.IndexOf(password[i]);
						if (c >= 0) {
							lang = CharacterSet.Latin;
						} else {
							c = BASE64_JP.IndexOf(password[i]);
							lang = CharacterSet.Japanese;
						}
						break;
				}

				// Fail if decoding is unsuccessful.
				if (c < 0) {
					//Console.Error.WriteLine("Base64 decode failed: invalid character \'" + password[i] + "\'.");
					return false;
				}

				bytes[i] = (byte)c;
			}

			// Password decoded successfully; create password data.
			data = new BoktaiDSPassword(bytes);

			// Fail if checksum mismatch.
			int checksum = data.ReadBits(0, 16);
			if (checksum != data.CalcChecksum()) {
				//Console.Error.WriteLine("Checksum mismatch.");
				return false;
			}

			// Decrypt the password data.
			data.Decrypt();

			return true;
		}

		/// <summary>
		/// Validates all of this password data's properties.
		/// </summary>
		/// <returns>true if the password data is valid; otherwise, false.</returns>
		public bool Validate() {
			// Fail if padding nonzero.
			if (this.ReadBits(16, 2) != 0) {
				//Console.Error.WriteLine("Padding 1 nonzero.");
				return false;
			}
			if (this.ReadBits(234, 0) != 0) {
				//Console.Error.WriteLine("Padding 2 nonzero.");
				return false;
			}

			// Fail if certain variables out of range.
			if (this.Region == GameRegion.Invalid) {
				//Console.Error.WriteLine("Region out of range.");
				return false;
			}
			if (this.Difficulty == GameDifficulty.Invalid) {
				//Console.Error.WriteLine("Difficulty out of range.");
				return false;
			}
			if (this.FavoriteSword == Sword.Invalid) {
				//Console.Error.WriteLine("Favorite sword out of range.");
				return false;
			}
			if (this.FavoriteGun == Gun.Invalid) {
				//Console.Error.WriteLine("Favorite gun out of range.");
				return false;
			}
			if (this.FavoriteTerrennial == Terrennial.Invalid) {
				//Console.Error.WriteLine("Favorite Terrennial out of range.");
				return false;
			}
			if (this.FavoriteClimate == Climate.Invalid) {
				//Console.Error.WriteLine("Favorite climate out of range.");
				return false;
			}

			// Fail if name characters invalid.
			if (!this.ValidateString(74, 10)) {
				//Console.Error.WriteLine("Invalid chars in Sabata name.");
				return false;
			}
			if (!this.ValidateString(154, 10)) {
				//Console.Error.WriteLine("Invalid chars in Django name.");
				return false;
			}

			return true;
		}

		private void Decrypt() {
			// Clear checksum bytes.
			for (int i = 0; i < 3; i++) {
				this.Bytes[i] = 0;
			}

			// Read offset, init keys.
			int offset = ReadBits(21, 3);
			int key1 = 0x5BB15;
			int key2 = 0xFFFF;

			// Decrypt bytes.
			for (int i = -offset; i < PASS_LEN; i++) {
				key1 = key1 * 0x6262C05D + 1;
				if (i >= 4) {
					this.Bytes[i] = (byte)((this.Bytes[i] ^ key2) & 0x3F);
				}
				key2 ^= key1;
			}
		}

		private int CalcChecksum() {
			int checksum = -1;
			for (int i = 3; i < PASS_LEN; i++) {
				checksum ^= this.Bytes[i] << 8;

				for (int j = 0; j < 8; j++) {
					checksum <<= 1;
					checksum ^= 0x180D * (checksum >> 16 & 1);
				}
			}

			return ~checksum & 0xFFFF;
		}

		private int ReadBits(int offset, int count) {
			int value = 0;
			for (int i = 0; i < count; i++) {
				value |= ((this.Bytes[(offset + i) / CHAR_WIDTH] >> (offset + i) % CHAR_WIDTH) & 1) << i;
			}

			return value;
		}

		private void WriteBits(int offset, int count, int value) {
			for (int i = 0; i < count; i++) {
				this.Bytes[(offset + i) / CHAR_WIDTH] |= (byte)((value & 1) << ((offset + i) % CHAR_WIDTH));
				value <<= 1;
			}
		}

		private string ReadString(int offset, int maxLength) {
			string str = "";
			char c = (char)0;
			for (int i = 0; i < maxLength; i++) {
				int pos = offset + i * 8;

				switch (this.Region) {
					case GameRegion.Japan:
						c = ReadCharJapanese(pos);
						i++;
						break;
					case GameRegion.NorthAmerica:
					case GameRegion.Europe:
						c = ReadCharEnglish(pos);
						break;
				}

				if (c == 0) {
					break;
				}

				str += c;
			}

			return str;
		}

		private char ReadCharEnglish(int offset) {
			int c = ReadBits(offset, 8);
			if (c == 0) {
				return (char)0;
			}

			return TABLE[c];
		}

		private char ReadCharJapanese(int offset) {
			int c = ReadBits(offset, 8) << 8;
			c += ReadBits(offset + 8, 8);
			if (c == 0) {
				return (char)0;
			}

			// Switch character table.
			switch (c >> 8) {
				case 0x80:
					return TABLE_80[c & 0xFF];
				case 0x86:
					return TABLE_86[c & 0xFF];
				default:
					// Return Unicode replacement char.
					return '\uFFFD';
			}
		}

		private bool ValidateString(int offset, int maxLength) {
			// Read the string.
			string str = ReadString(offset, maxLength);

			// Check for invalid chars.
			if (str.IndexOf('\uFFFD') >= 0) {
				return false;
			}

			// Calculate number of bytes this string occupies.
			int byteCount;
			switch (this.Region) {
				case GameRegion.Japan:
					byteCount = str.Length * 2;
					break;
				case GameRegion.NorthAmerica:
				case GameRegion.Europe:
					byteCount = str.Length;
					break;
				default:
					byteCount = 0;
					break;
			}

			// Check that the rest of the bytes are zero.
			for (int i = byteCount; i < maxLength; i++) {
				if (ReadBits(offset + i * 8, 8) != 0) {
					return false;
				}
			}

			return true;
		}

		/*public override string ToString() {
			string password = "";

			int offset = new Random().Next(8);
			this.WriteBits(21, 3, offset);

			// TODO: everything else lol

			return password;
		}*/
	}
