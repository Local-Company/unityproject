using UnityEngine;

namespace Logger {
    public class Unity_Logger : ILogger {
        public void Server(string log) {
            Debug.Log($"[SERVER] {log}");
        }

        public void Client(string log) {
            Debug.Log($"[CLIENT] {log}");
        }
    }
}