# OatmealDome.NinLib.Nisasyst

An implementation of Splatoon 2's nisasyst encryption in C#.

## Usage

```csharp
byte[] encryptedData;
byte[] decryptedData = NisasystDecryptor.Decrypt(encryptedData, "Game/Path/Encrypted.bin");
```
