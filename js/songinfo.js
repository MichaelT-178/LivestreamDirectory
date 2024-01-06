window.addEventListener('load', () => {

    const title = localStorage.getItem("theTitle");
    const artist = localStorage.getItem("theArtist");
    const other_artists = localStorage.getItem("theOtherArt");
    const appears = localStorage.getItem("Appears");
    let instruments = localStorage.getItem("theInstruments");
    const image = `../pics/${localStorage.getItem("theImage")}`;
    const theLinks = localStorage.getItem("theLinks");

    let theTitle = title + (title.includes("Session #") ? " (Check comments for full timestamp)" : "");

    let numberOfH;
    let hString;

    //if condition is true add h's for length purposes to push thing to the left over so it's not dead center
    //Kind of a ghetto fix but it works.
    if (theTitle.length < 28 && artist.length < 28 && other_artists.length < 28 && instruments.length < 28) {
        numberOfH = 27 - theTitle.length;
        hString = 'h'.repeat(Math.max(numberOfH, 0));
    }

    instruments = `${instruments}<span style='color: black; user-select: none;'>${hString || ""}</span>`; 

    document.getElementById('result-title').innerHTML = ": " + theTitle;
    document.getElementById('result-artist').innerText = ": " + artist;
    document.getElementById('result-otherart').innerText = ": " + (other_artists || "N/A");
    document.getElementById('result-instruments').innerHTML = ": " + instruments;


    const instrumentList = instruments.split(",");

    const marginLeft = instrumentList.length > 2 ? 34 : 8;

    const mediaQuery = window.matchMedia('(min-device-width: 375px) and (max-device-width: 812px)');
    const instrumentStyling = "font-size: 24px;" + 
                              "color: lightBlue;" +
                              "text-align: left;" +
                              `margin-left: ${marginLeft}px;` +
                              "margin-bottom: 9px;";

    if (instrumentList.length > 2 && !mediaQuery.matches) {

        if (theTitle.length < 30 && artist.length < 28 && other_artists.length < 28) {
            numberOfH = 30 - theTitle.length;
            hString = 'h'.repeat(Math.max(numberOfH, 0));
        }
    
        theTitle = `${theTitle}<span style='color: black; user-select: none;'>${hString || ""}</span>`; 
        document.getElementById('result-title').innerHTML = "";
        document.getElementById('result-title').innerHTML = ": " + theTitle;
        
        document.getElementById('result-instruments').innerHTML = "";
        document.getElementById('result-instrument').innerHTML = instrumentList.map((instrument) => 
        ` <div style="${instrumentStyling}"> • ${instrument} </div>`
        ).join(''); 

        document.getElementById('result-instrument').style.marginTop = '-11px';

    }

                              
    if (mediaQuery.matches) {
        document.getElementById('result-instruments').innerHTML = "";
        
        document.getElementById('result-instrument').innerHTML = instrumentList.map((instrument) => 
        ` <div style="${instrumentStyling}"> • ${instrument} </div>`
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