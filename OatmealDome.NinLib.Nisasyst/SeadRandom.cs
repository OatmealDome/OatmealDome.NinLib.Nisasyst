namespace OatmealDome.NinLib.Nisasyst;

internal sealed class SeadRandom
{
    private readonly uint[] _internalData;

    public SeadRandom(uint seed)
    {
        _internalData = new uint[4];
        _internalData[0] = 1812433253 * (seed ^ (seed >> 30)) + 1;
        _internalData[1] = 1812433253 * (_internalData[0] ^ (_internalData[0] >> 30)) + 2;
        _internalData[2] = 1812433253 * (_internalData[1] ^ (_internalData[1] >> 30)) + 3;
        _internalData[3] = 1812433253 * (_internalData[2] ^ (_internalData[2] >> 30)) + 4;
    }

    public SeadRandom(uint seedOne, uint seedTwo, uint seedThree, uint seedFour)
    {
        _internalData = new uint[] { seedOne, seedTwo, seedThree, seedFour };
    }

    public uint GetUInt32()
    {
        uint v1;
        uint v2;
        uint v3;

        v1 = _internalData[0] ^ (_internalData[0] << 11);
        _internalData[0] = _internalData[1];
        v2 = _internalData[3];
        v3 = v1 ^ (v1 >> 8) ^ v2 ^ (v2 >> 19);
        _internalData[1] = _internalData[2];
        _internalData[2] = v2;
        _internalData[3] = v3;

        return v3;
    }
}
