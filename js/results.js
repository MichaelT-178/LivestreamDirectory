window.addEventListener('load', () => {

    const title = localStorage.getItem("theTitle");
    const artist = localStorage.getItem("theArtist");
    const other_artists = localStorage.getItem("theOtherArt");
    const appears = localStorage.getItem("Appears");
    const instruments = localStorage.getItem("theInstruments");
    const image = localStorage.getItem("theImage");
    const theLinks = localStorage.getItem("theLinks");

    theTitle = title.replace(" (Classical Guitar)", "").replace(" (Electric Song)", "");
    theTitle += title.includes("Session #") ? " (Check comments for full timestamp)" : "";

    document.getElementById('result-title').innerText = ": " + theTitle;
    document.getElementById('result-artist').innerText = ": " + artist;
    document.getElementById('result-otherart').innerText = ": " + (other_artists || "N/A");
    document.getElementById('result-instruments').innerText = ": " + instruments;
    
    const mediaQuery = window.matchMedia('(min-device-width: 375px) and (max-device-width: 812px)');
    
    if (mediaQuery.matches) {
        document.getElementById('result-instruments').innerHTML = "";

        document.getElementById('result-instrument').innerHTML = instruments.split(",").map((instrument) => 
        ` <div style="font-size:24px;margin: 6px 0; left: 5px;margin-left: 33px;"> • ${instrument} </div>`
        ).join(''); 
    }; 
    
    window.addEventListener('error', () => {
        console.error("Image not found");
        console.error("Image name: " + image);
    }, true);

    document.getElementById("pic1").src = image;

    web = appears.split(",");

    const alllinks = theLinks.split(" , ").map((link, i) => 
    `<div style="margin: 10px 0;">• <a href="${link}" style= "font-size: 25px;text-decoration: none;">${web[i]}</a></div>`
    ).join('');

    document.getElementById('result-links').innerHTML = alllinks;

});