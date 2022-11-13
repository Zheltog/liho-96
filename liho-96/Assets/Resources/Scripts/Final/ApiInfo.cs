using System;

namespace Final
{
    public static class ApiInfoHolder
    {
        public const string QuestDomain = "https://fitfija.ru";
        public const string LoginPath = "/api/v1/auth/login";
        public const string CheckTaskPath = "/api/v1/tasks/check";
        public const string TaskId = "22";
    }

    [Serializable]
    public class LoginRequest {
        public string email;
        public string password;

        public LoginRequest(string email, string password) {
            this.email = email;
            this.password = password;
        }
    }
    
    [Serializable]
    public class LoginResponse {
        public string token { get; set; }
    }
    
    [Serializable]
    public class CheckTaskRequest {
        public string taskId;
        public string answer;

        public CheckTaskRequest(string taskId, string answer) {
            this.taskId = taskId;
            this.answer = answer;
        }
    }
    
    [Serializable]
    public class CheckTaskResponse {
        public int? points { get; set; }
    }
}