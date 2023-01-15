const loginForm = document.querySelector("#login-form");
const loginFormSbmBtn = document.querySelector("#login-form-submit");
const errorMessage = document.querySelector(".error-message");

async function sendData() {
  let data = new FormData(loginForm);
  let obj = {};

  data.forEach((value, key) => {
    obj[key] = value;
  });

  // mainUrl Iš config.js failo
  const response = await fetch(mainUrl + "Login", {
    method: "post",
    headers: {
      Accept: "application/json, text/plain, */*",
      "Content-Type": "application/json",
    },
    // Naudojame JSON.stringify, nes objekte neturim .json() metodo
    body: JSON.stringify(obj),
  });

  if (response.ok) {
    const data = await response.json();
    console.log("res: ", data);
    localStorage.setItem("role", data.role);
    localStorage.setItem("user-token", data.token);
    document.location.reload(true);
  } else {
    document.querySelector(".error-message").innerText = "Wrong username or password";
  }
}

loginFormSbmBtn.addEventListener("click", (e) => {
  e.preventDefault(); // Breaks manual refresh after submit
  sendData();
});
