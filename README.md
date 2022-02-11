# OatmealDome.NinLib.Nisasyst

An implementation of Splatoon 2's nisasyst encryption in C#.

## Usage

```csharp
byte[] encryptedData = File.ReadAllBytes("Encrypted.bin");
byte[] decryptedData = NisasystDecryptor.Decrypt(encryptedData, "Game/Path/Encrypted.bin");
```
