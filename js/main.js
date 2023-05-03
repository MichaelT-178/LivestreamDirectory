const search = document.getElementById('search');
const matchList = document.getElementById('match-list');
let otherMatches = [];

let JSONdata;
let songs;

const getSongs = async () => {
    const response = await fetch('./json/database/song_list.json');
    JSONdata = await response.json();
    songs = JSONdata['songs'];
};

const searchSongs = async searchText => {

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
               song.Other.toLowerCase().match(search)) || 

               //Allows the user to look "Song by Artist". Example As by Stevie Wonder is now 
               //easier to find. The double equals allows for ascii characters
               ((song.Title.toLowerCase() == songText.toLowerCase()) || 
               (song.Artist.toLowerCase() == artistText.toLowerCase()));
    });
    
    const l = document.getElementById('link');
    const l2 = document.getElementById('helplink');

    //Hides the "Contact the developer" and "help" button when searching
    l.style.display  = matches.length === 0 || searchText.length === 0 ? "flex" : "none";
    l2.style.display = matches.length === 0 || searchText.length === 0 ? "flex" : "none";

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
        <a href="indexTwo.html" style="text-decoration:none;">
            <button type="button"  style="background-color:#222; width: 100%; overflow: hidden;"> 
            

                <h4>${match.Title} by <span class="text-primary">
                ${match.Artist}</span></h4>
                <small style="color:#FFFFFF">Instruments: ${match.Instruments} / Other Artists: 
                ${(match.Other_Artists === "") ? "N/A" : match.Other_Artists}</small>
                

                </button>
                </a>
            </div>

        `).join('');

    matchList.innerHTML = html;
    
  };
};

// Line 51. Code below will make button invisible 
//<button type="button" style="background:transparent; border:none; color:transparent; width: 100%; overflow: hidden;">

matchList.addEventListener('click', (e) => {
    const button = e.target.closest('button');

    if (button) {
        const card = button.closest('.card');
        const id = card.dataset.id;
        matchList.innerText = "";   

        localStorage.setItem("theTitle", otherMatches[id].Title);
        localStorage.setItem("theArtist", otherMatches[id].Artist);
        localStorage.setItem("theOtherArt", otherMatches[id].Other_Artists);
        localStorage.setItem("Appears", otherMatches[id].Appearances);
        localStorage.setItem("theInstruments", otherMatches[id].Instruments);
        localStorage.setItem("theImage", otherMatches[id].Image);
        localStorage.setItem("theLinks", otherMatches[id].Links);

        document.getElementById('search').value = "";
        document.getElementById('link').style.display = "flex";
        document.getElementById('helplink').style.display = "flex";
    };
});

window.addEventListener('DOMContentLoaded', getSongs);
search.addEventListener('input', () => searchSongs(search.value));