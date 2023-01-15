if (localStorage.getItem("user-token") && localStorage.getItem("role")) {
  // jeigu prisijungęs nauduotojas, tada paslepia formą ir rodo, kad esate prisijungę
  if (window.location.pathname.includes("login") || window.location.pathname.includes("register")) {
    if (document.querySelector(".login-container")) {
      document.querySelector(".login-container").style.display = "none";
    }
    if (document.querySelector(".register-container")) {
      document.querySelector(".register-container").style.display = "none";
    }
    document.querySelector(".auth-message").style.display = "block";
  }

  // parodo logout linką
  if (document.querySelector(".logout")) {
    document.querySelector(".logout").style.display = "inline";

    // logout funkciją
    document.querySelector(".logout").addEventListener("click", () => {
      localStorage.removeItem("role");
      localStorage.removeItem("user-token");
      document.location.reload(true);
    });
  }

  // Jeigu prisijungęs, nemato logino ir registracijos linkų
  if (document.querySelector(".login")) {
    document.querySelector(".login").style.display = "none";
  }

  if (document.querySelector(".register")) {
    document.querySelector(".register").style.display = "none";
  }

  // Parodyti puslapius pagal tam tikras roles
  const role = localStorage.getItem("role");

  if (role === "Manager" || role === "Administrator") {
    document.querySelector(".customers").style.display = "inline";
  }

  if (role === "Manager" || role === "Administrator" || role === "Supervisor") {
    document.querySelector(".inventory").style.display = "inline";
    document.querySelector(".orders").style.display = "inline";
    document.querySelector(".products").style.display = "inline";
  }
} else {
  // jeigu neprisijungęs vartotojas nori pasiekti tam tikrus puslapius
  if (
    window.location.pathname.includes("customers") ||
    window.location.pathname.includes("inventory") ||
    window.location.pathname.includes("orders") ||
    window.location.pathname.includes("products") ||
    window.location.pathname.includes("cart")
  ) {
    document.querySelector("main").style.display = "none";
  }
}
