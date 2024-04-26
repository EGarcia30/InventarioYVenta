const btnEye = document.getElementById('btnEye');
const iconEye = document.getElementById('iconEye');
const inputPassword = document.getElementById('Password');

const handlePassword = () =>{
    if(iconEye.classList.contains('fa-eye')){
        inputPassword.setAttribute('type','text');
        iconEye.classList.replace('fa-eye','fa-eye-slash');
    }
    else{
        inputPassword.setAttribute('type','password');
        iconEye.classList.replace('fa-eye-slash','fa-eye');
    }
}

btnEye.addEventListener('click', handlePassword);