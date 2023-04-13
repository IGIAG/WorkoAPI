namespace WorkoAPI;

public static class Logger {
    private readonly static string transactionLogPath = "./tranactions.log";
    
    public static void Init(){
        if(!System.IO.File.Exists(transactionLogPath)){System.IO.File.Create(transactionLogPath);}
    }
    public static void LogTransaction(string fromUserId,string targetUserId,int amount){
        System.IO.File.AppendAllText(transactionLogPath,"\nTransaction: " + fromUserId + " => " + targetUserId + " ( " + amount + " )");
    }
}