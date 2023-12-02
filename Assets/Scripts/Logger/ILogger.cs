namespace Logger {
    public interface ILogger {
        void Server(string log);
        void Client(string log);
    }
}