namespace SMSnew
{
    internal class SerialDataReceivedEvenHandler
    {
        private object dataReceivedHandler;

        public SerialDataReceivedEvenHandler(object dataReceivedHandler)
        {
            this.dataReceivedHandler = dataReceivedHandler;
        }
    }
}