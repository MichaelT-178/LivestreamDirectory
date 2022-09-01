const btn = document.getElementById('button');

document.getElementById('form').addEventListener('submit', function(event) {
   event.preventDefault();

   btn.value = 'Sending...';

   const serviceID = 'default_service';
   const templateID = 'template_ngfg5ko';
   const SentOrNot = document.getElementById('SentOrNot');
   SentOrNot.innerHTML = '';

   emailjs.sendForm(serviceID, templateID, this) 
    .then(() => {
      SentOrNot.innerHTML =  
      "<span id='sent'> "+  
      "Email sent succesfully!</span>";
      
      setTimeout(function () {
        SentOrNot.innerHTML = '';
        btn.value = 'Send Email';
      }, 3500); 

    }, (err) => {
      SentOrNot.innerHTML =  
      "<span id='err'> "+
      "Email not sent! Error occured!</span>";

      setTimeout(function () {
        SentOrNot.innerHTML = '';
        btn.value = 'Send Email';
      }, 5000);

      console.log(err)
    });
});