# OatmealDome.NinLib.Nisasyst

An implementation of Splatoon 2's nisasyst encryption and Splatoon 3's nisasyst encryption for BCAT in C#.

## Usage

```csharp
// Decrypting Splatoon 2 data
byte[] encryptedData;
byte[] decryptedData = NisasystDecryptor.Decrypt(encryptedData, "Game/Path/Encrypted.bin");

// Decrypting Splatoon 3 BCAT data
byte[] encryptedData;
byte[] decryptedData = NisasystDecryptor.DecryptThunderBcat(encryptedData, "resourceId", "fileName", encryptionKey);
```
