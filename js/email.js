const btn = document.getElementById('button');

document.getElementById('form').addEventListener('submit', function(event) {
   event.preventDefault();

   btn.value = 'Sending...';

   const serviceID = 'default_service';
   const templateID = 'template_ngfg5ko';

   emailjs.sendForm(serviceID, templateID, this) 
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