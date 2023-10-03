namespace KSociety.SharpCubeProgrammer.Enum
{
    public enum UsartFlowControl
    {
        /// <summary>
        /// No flow control.
        /// </summary>
        Off = 0,

        /// <summary>
        /// Hardware flow control : RTS/CTS.
        /// </summary>
        Hardware = 1,

        /// <summary>
        /// Software flow control : Transmission is started and stopped by sending special characters.
        /// </summary>
        Software = 2
    }
}
