function like(){
    document.querySelectorAll('.heart, .heart-button').forEach(button => 
        button.addEventListener('click', () =>
        button.classList.toggle('active')));
    }