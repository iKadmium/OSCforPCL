namespace OSCforPCL
{
    public interface IOSCValue
    {
        char TypeTag { get; }
        byte[] Bytes { get; }
    }

    public interface IOSCValue<T> : IOSCValue
    {
        T Contents { get; }
    }
}
