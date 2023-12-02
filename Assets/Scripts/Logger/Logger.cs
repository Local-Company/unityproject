namespace Logger {
    public class Logger {
        public static ILogger createLogger() {
            return new Unity_Logger();
        }
    }
}