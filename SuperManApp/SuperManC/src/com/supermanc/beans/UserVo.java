package com.supermanc.beans;

public class UserVo {

    private int Status;
    private String Message;
    private User Result;

    public int getStatus() {
        return Status;
    }

    public void setStatus(int status) {
        Status = status;
    }

    public String getMessage() {
        return Message;
    }

    public void setMessage(String message) {
        Message = message;
    }

    public User getResult() {
        return Result;
    }

    public void setResult(User result) {
        Result = result;
    }

    @Override
    public String toString() {
        return "UserVo [Status=" + Status + ", Message=" + Message + ", Result=" + Result + "]";
    }

    public class User {
        private String userId;
        private String userName;
        private String password;
        private int status;// 0未通过 1通过 2审核未通过
        private String Amount;

        public int getStatus() {
            return status;
        }

        public void setStatus(int status) {
            this.status = status;
        }

        public String getAmount() {
            return Amount;
        }

        public void setAmount(String amount) {
            Amount = amount;
        }

        public String getUserId() {
            return userId;
        }

        public void setUserId(String userId) {
            this.userId = userId;
        }

        public String getUserName() {
            return userName;
        }

        public void setUserName(String userName) {
            this.userName = userName;
        }

        public String getPassword() {
            return password;
        }

        public void setPassword(String password) {
            this.password = password;
        }

        @Override
        public String toString() {
            return "User [userId=" + userId + ", userName=" + userName + ", status=" + status + ", Amount=" + Amount
                    + "]";
        }
    }

}
