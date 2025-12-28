using SimpleSQL;


namespace Studio.Data {

public class DataStore {


    // ------------------- SYSTEM REPOSITORIES -------------------
    public Repository<Settings> Settings { get; private set; }
    public Repository<Tags> Tags { get; private set; }
    public Repository<TagLinks> TagLinks { get; private set; }


    // ------------------- PROJECT REPOSITORIES -------------------
    public Repository<Media> Media { get; private set; }
    public Repository<AIPrompts> AIPrompts { get; private set; }

    public Repository<Characters> Characters { get; private set; }
    public Repository<CharacterData> CharacterData { get; private set; }
    public Repository<CharacterLinks> CharacterLinks { get; private set; }



    public void init(SimpleSQLManager db){
        Settings = new Repository<Settings>(db);
        Tags = new Repository<Tags>(db);
        TagLinks = new Repository<TagLinks>(db);

        Media = new Repository<Media>(db);
        AIPrompts = new Repository<AIPrompts>(db);

        Characters = new Repository<Characters>(db);
        CharacterData = new Repository<CharacterData>(db);
        CharacterLinks = new Repository<CharacterLinks>(db);

        
        UnityEngine.Debug.Log("[DataStore] Repositories Initialized for Active Project.");
    }

    public void Clear(){
        Settings = null;
        Tags = null;
        TagLinks = null;

        Media = null;
        AIPrompts = null;

        Characters = null;
        CharacterData = null;
        CharacterLinks = null;
    }


}
}