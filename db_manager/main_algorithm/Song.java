package db_manager.main_algorithm;

public class Song {

    private String title;
    private String artist;
    private String appearances;
    private String instruments;
    private String image;
    private String links;

    public Song(String title, String artist, String appearances, String instruments, String image, String links) {
        this.title = title;
        this.artist = artist;
        this.appearances = appearances;
        this.instruments = instruments;
        this.image = image;
        this.links = links;
    }

    public String getTitle() {
        return title;
    }

    public String getArtist() {
        return artist;
    }

    public String getAppearances() {
        return appearances;
    }

    public String getInstruments() {
        return instruments;
    }

    public String getImage() {
        return image;
    }

    public String getLinks() {
        return links;
    }
    
}
