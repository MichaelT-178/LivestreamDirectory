const search = document.getElementById('search');
const matchList = document.getElementById('match-list');
let otherMatches = [];

const getSongs = async () => {
    try {
        const response = await fetch('./database/song_list.json');
        const data = await response.json();
        return data['songs'];
    } catch (error) {
        console.error('Error fetching data:', error);
    }
};

const searchSongs = async searchText => {

    let songs = await getSongs();

    let matches = songs.filter(song => {
        //const regex = new RegExp(`${searchText.toLowerCase().trim()}`, 'gi')

        let songText = ""
        let artistText = ""

        //Allows the user to look up "Song by Artist". Ignores life by the drop, Train in Vain (Stand by Me),
        //Down by the River
        if (searchText.includes(" by") && !searchText.includes(" drop") && 
            !searchText.includes("vain") && !searchText.includes("by the River")) {
                songText = searchText.split(" by ")[0]
                artistText = searchText.split(" by ")[1]
        }


        const search = searchText.toLowerCase().trim();

        return (song.Title.toLowerCase().match(search) || 
               song.Artist.toLowerCase().match(search) || 
               song.Other_Artists.toLowerCase().match(search) || 
               song.Instruments.toLowerCase().match(search) || 
               song.Search.toLowerCase().match(search)) || 

               //Allows the user to look "Song by Artist". Example As by Stevie Wonder is now 
               //easier to find. The double equals allows for ascii characters
               ((song.Title.toLowerCase() == songText.toLowerCase()) || 
               (song.Artist.toLowerCase() == artistText.toLowerCase()));
    });
    
    const l = document.getElementById('link');
    const l2 = document.getElementById('helplink');

    // DELETE THIS EVENUTALLY AND l3.style.display
    const l3 = document.getElementById('new-website-link')

    //Hides the "Contact the developer" and "help" button when searching
    l.style.display  = matches.length === 0 || searchText.length === 0 ? "flex" : "none";
    l2.style.display = matches.length === 0 || searchText.length === 0 ? "flex" : "none";
    l3.style.display = matches.length === 0 || searchText.length === 0 ? "flex" : "none";

    if (searchText.length === 0 || matches.length === 0) {
        matches = [];
        const thehtml = `No results for: "${searchText}"`;
        matchList.innerText = (searchText === "") ? '' : thehtml;
    };

    outputHtml(matches);
    otherMatches = matches;
};


const outputHtml = matches => {
    if (matches.length > 0) {
        const html = matches.map((match, i) => 
       
        `<div class="card card-body mb-1" data-id="${i}">
        <a href="/html/SongInfo.html" style="text-decoration:none;">
            <button type="button"  style="background-color:#222; width: 100%; overflow: hidden;"> 
            

                <h4>${match.Title} by <span class="text-primary">
                ${match.Artist}</span></h4>
                <small style="color:#FFFFFF">Instruments: ${
                    match.Instruments
                        //   .replace("(main) - Stonebridge (Furch) OM32SM, ", "")
                        //   .replace("Fender Telecaster, ", "")
                          .split(', ')
                          .slice(0, 3)
                          .join(', ') 
                          + (match.Instruments.split(', ').length > 3 ? "..." : "")
                } / Other Artists: 
                ${(match.Other_Artists === "") ? "N/A" : match.Other_Artists}</small>
                

                </button>
                </a>
            </div>

        `).join('');

    matchList.innerHTML = html;
    
  };
};

const goToSongInfo = (id) => {
    const match = otherMatches[id];

    localStorage.setItem("theTitle", match.Title);
    localStorage.setItem("theArtist", match.Artist);
    localStorage.setItem("theOtherArt", match.Other_Artists);
    localStorage.setItem("Appears", match.Appearances);
    localStorage.setItem("theInstruments", match.Instruments);
    localStorage.setItem("theImage", match.CleanedArtist);
    localStorage.setItem("theLinks", match.Links);

    document.getElementById('search').value = "";
    document.getElementById('link').style.display = "flex";
    document.getElementById('helplink').style.display = "flex";
};


matchList.addEventListener('click', (e) => {
    const button = e.target.closest('button');

    if (button) {
        const card = button.closest('.card');
        const id = card.dataset.id;
        matchList.innerText = "";
        goToSongInfo(id);
    }
});


//If the user presses return/enter when there is 1 item in the list it will go 
//to the songinfo page.
search.addEventListener('keyup', (e) => {
    if (e.key === "Enter") {
        goToSongInfo(0);
        matchList.innerText = "";
        window.location.href = './html/SongInfo.html';
    }
});


window.addEventListener('DOMContentLoaded', getSongs);
search.addEventListener('input', () => searchSongs(search.value));
