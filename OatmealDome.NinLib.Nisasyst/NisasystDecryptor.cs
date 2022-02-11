using System.Security.Cryptography;
using System.Text;
using Force.Crc32;

namespace OatmealDome.NinLib.Nisasyst;

public sealed class NisasystDecryptor
{
    private const string KeyMaterialString = "e413645fa69cafe34a76192843e48cbd691d1f9fba87e8a23d40e02ce13b0d534d10301576f31bc70b763a60cf07149cfca50e2a6b3955b98f26ca84a5844a8aeca7318f8d7dba406af4e45c4806fa4d7b736d51cceaaf0e96f657bb3a8af9b175d51b9bddc1ed475677260f33c41ddbc1ee30b46c4df1b24a25cf7cb6019794";
    private static readonly byte[] MagicNumbers = Encoding.ASCII.GetBytes("nisasyst");

    public static byte[] Decrypt(byte[] data, string gamePath)
    {
        if (gamePath.StartsWith('/'))
        {
            gamePath = gamePath.Substring(1);
        }
        
        using MemoryStream inputStream = new MemoryStream(data);
        using MemoryStream outputStream = new MemoryStream();
        
        Decrypt(inputStream, outputStream, gamePath);

        return outputStream.ToArray();
    }

    public static void Decrypt(Stream inputStream, Stream outputStream, string gamePath)
    {
        if (!IsNisasystFile(inputStream))
        {
            throw new InvalidDataException("Not a nisasyst-encrypted file");
        }

        if (gamePath.StartsWith('/'))
        {
            gamePath = gamePath.Substring(1);
        }
        
        // Read the entire file into an array
        byte[] encryptedData = new byte[inputStream.Length - 8];
        inputStream.Seek(0, SeekOrigin.Begin);
        inputStream.Read(encryptedData, 0, (int)inputStream.Length - 8);

        // Generate a CRC32 over the game path
        uint seed = Crc32Algorithm.Compute(Encoding.ASCII.GetBytes(gamePath));

        // Create a new SeadRandom instance using the seed
        SeadRandom seadRandom = new SeadRandom(seed);

        using (Aes aes = Aes.Create())
        {
            aes.Mode = CipherMode.CBC;
            aes.Key = CreateSequence(seadRandom);
            aes.IV = CreateSequence(seadRandom);
            aes.Padding = PaddingMode.None;

            using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateDecryptor(), CryptoStreamMode.Write, true))
            {
                cryptoStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        // Seek back to the beginning
        outputStream.Seek(0, SeekOrigin.Begin);
    }

    public static bool IsNisasystFile(Stream stream)
    {
        // Check length
        if (stream.Length <= 8)
        {
            return false;
        }

        // Read the magic numbers
        byte[] lastBytes = new byte[8];
        stream.Seek(stream.Length - 8, SeekOrigin.Begin);
        stream.Read(lastBytes, 0, 8);

        // Seek back to the beginning
        stream.Seek(0, SeekOrigin.Begin);

        return lastBytes.SequenceEqual(MagicNumbers);
    }

    private static byte[] CreateSequence(SeadRandom random)
    {
        // Create byte array
        byte[] sequence = new byte[16];

        // Create each byte
        for (int i = 0; i < sequence.Length; i++)
        {
            // Create empty byte string
            string byteString = "";

            // Get characters from key material
            byteString += KeyMaterialString[(int)(random.GetUInt32() >> 24)];
            byteString += KeyMaterialString[(int)(random.GetUInt32() >> 24)];

            // Parse the resulting byte
            sequence[i] = Convert.ToByte(byteString, 16);
        }

        // Return the sequence
        return sequence;
    }
}
