public interface ICarInputs
{
    void ReadInputs();

    bool UP { get; }
    bool DOWN { get; }
    bool LEFT { get; }
    bool RIGHT { get; }
    bool DoubleTapRIGHT { get;}
    bool DoubleTapLEFT { get; }
    bool JumpLEFT { get; }
    bool JumpRIGHT { get; }
}
