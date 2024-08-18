const btn = document.getElementById('button');

document.getElementById('form').addEventListener('submit', function(event) {
   event.preventDefault();

   btn.value = 'Sending...';

   const serviceID = 'default_service';
   const templateID = 'template_cowiwh7';
   
   const templateParams = {
     subject: "Email from Heuvel Website",
     website: "the Livestream Directory.",
     from_name: "",
     from_email: "Person who sent this email",
     message: message
   };

   emailjs.send(serviceID, templateID, templateParams) 
    .then(() => {
        Swal.fire({
            title: 'Success!',
            text: 'Email sent successfully!',
            confirmButtonColor: '#4CAF50'
        });

        btn.value = 'Send Email';
        document.getElementById('box').value = ''; // Clear the textarea
    })
    .catch((err) => { 
        Swal.fire({
            title: 'Error!',
            text: 'Email not sent! Error occurred.',
            confirmButtonColor: '#FF5733'
        });
        
        btn.value = 'Send Email';
        console.log(err)
    });
});