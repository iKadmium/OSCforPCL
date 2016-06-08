namespace OSCforPCL.Values
{
    public interface IOSCValue
    {
        char TypeTag { get; }
        byte[] Bytes { get; }
        object GetValue();
    }

    public interface IOSCValue<T> : IOSCValue
    {
        T Contents { get; }
    }
}
