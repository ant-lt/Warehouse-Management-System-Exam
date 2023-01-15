const registrationForm = document.querySelector("#registration-form");
const registrationFormSbmBtn = document.querySelector("#registration-form-submit");
const errorEle = document.querySelector(".error-message");

async function sendData() {
  let data = new FormData(registrationForm);
  let obj = {};

  console.log(data);

  data.forEach((value, key) => {
    obj[key] = value;
  });

  obj.role = "Manager";

  // Neleidžiam įvesti password turinti mažiau nei 8 simbolius
  if (obj.password.length >= 8) {
    // mainUrl imam iš config.js failo
    const response = await fetch(mainUrl + "Register", {
      method: "post",
      headers: {
        Accept: "application/json, text/plain, */*",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(obj),
    });

    if (response.ok) {
      alert("Registered");
    } else {
      document.querySelector(".error-message").innerText = "Something went wrong";
    }
  } else {
    alert("Password should contain at least 8 characters");
  }
}
registrationFormSbmBtn.addEventListener("click", (e) => {
  e.preventDefault(); // Breaks manual refresh after submit
  sendData();
});
