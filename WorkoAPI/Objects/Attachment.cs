namespace WorkoAPI.Objects;

public class Attachment{
    public string id;

    public string mimeType;

    public string base64String;

    public Attachment(string id,string mimeType,string base64String) {
        this.id = id;
        this.mimeType = mimeType;
        this.base64String = base64String;
    }
}