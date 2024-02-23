function like(){
    console.log("функция активирована")
    document.querySelectorAll('.heart, .heart-button').forEach(button => 
        button.addEventListener('click', () =>
        button.classList.toggle('active')));
    }