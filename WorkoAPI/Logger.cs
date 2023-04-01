namespace WorkoAPI;

public static class Logger {
    private static string transactionLogPath = "./tranactions.log";
    
    public static void init(){
        if(!System.IO.File.Exists(transactionLogPath)){System.IO.File.Create(transactionLogPath);}
    }
    public static void logTransaction(string fromUserId,string targetUserId,int amount){
        System.IO.File.AppendAllText(transactionLogPath,"\nTransaction: " + fromUserId + " => " + targetUserId + " ( " + amount + " )");
    }
}