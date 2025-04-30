export const login = () => {
  const username = document.querySelector("#loginUsername");
  const password = document.querySelector("#loginPassword");
  const errorMessage = document.querySelector("#loginErrorMessage");
  const form = document.querySelector("#loginForm");

  // username.addEventListener("input", () => {
  //   const validatedName = validateName(username.value);

  //   if (!validatedName.isValid) {
  //     errorMessage.innerHTML = "Användarnamn måste anges";
  //   } else {
  //     errorMessage.innerHTML = "";
  //   }
  // });

  // password.addEventListener("input", () => {
  //   const validatedPassword = validatePassword(password.value);

  //   if (!validatedPassword.isValid) {
  //     errorMessage.innerHTML = "Lösenord måste anges";
  //   } else {
  //     errorMessage.innerHTML = "";
  //   }
  // });

  form.addEventListener("submit", (e) => {
    e.preventDefault();
    console.log("entering submit");

    const user = validateLogin(username.value, password.value, errorMessage);

    if (user === undefined) {
      console.log("user undefined");
      return;
    }

    fetch("https://localhost:7264/api/users/login", {
      method: "POST",
      body: JSON.stringify(user),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    }).then(async (response) => {
      const data = await parseResponse(response);

      if (response.ok) {
        console.log(data);
        console.log(response);
        
        //navigate("")
      } else {
        console.log(data);
        if (Array.isArray(data.errors)) {
          const messages = data.errors.map((err) => err).join("<br>");
          errorMessage.innerHTML = messages;
        } else if (typeof data === "string") {
          errorMessage.innerHTML = data;
        } else {
          errorMessage.innerHTML = "Ett okänt fel inträffade.";
        }
      }
    });
  });
};

const validateLogin = (username, password, errorMessage) => {
  const validatedUsername = validateName(username);
  const validatedPassword = validatePassword(password);

  if (!validatedUsername.isValid || !validatedPassword.isValid) {
    errorMessage.innerHTML = "Fel";
    return;
  }
  return { Username: username, Password: password };
};

const parseResponse = async (response) => {
  let data;
  const contentType = response.headers.get("content-type");

  // check format of the response to parse it appropriately
  if (contentType?.includes("application/json")) {
    data = await response.json();
  } else {
    data = await response.text();
  }

  return data;
};

export const registerAccount = () => {
  const emailInput = document.querySelector("#email");
  const emailLabel = document.querySelector("#emailFeedback");
  const nameInput = document.querySelector("#name");
  const nameLabel = document.querySelector("#nameFeedback");
  const passwordInput = document.querySelector("#password");
  const passwordLabel = document.querySelector("#passwordFeedback");
  const repeatPasswordInput = document.querySelector("#repeatPassword");
  const repeatPasswordLabel = document.querySelector("#repeatPasswordFeedback");

  const successMessage = document.querySelector("#successMessage");
  const errorMessage = document.querySelector("#errorMessage");
  const form = document.querySelector("#registerForm");

  emailInput.addEventListener("input", () => {
    const validatedEmail = validateEmail(emailInput.value);

    if (!validatedEmail.isValid) {
      emailLabel.innerHTML = validatedEmail.message;
    } else {
      emailLabel.innerHTML = "";
    }
  });

  nameInput.addEventListener("input", () => {
    const validatedName = validateName(nameInput.value);

    if (!validatedName.isValid) {
      nameLabel.innerHTML = validatedName.message;
    } else {
      nameLabel.innerHTML = "";
    }
  });

  passwordInput.addEventListener("input", () => {
    const validatedPassword = validatePassword(passwordInput.value);

    if (!validatedPassword.isValid) {
      passwordLabel.innerHTML = validatedPassword.message;
    } else {
      passwordLabel.innerHTML = "";
    }
  });

  repeatPasswordInput.addEventListener("input", () => {
    const validatedRepeatPassword = validateRepeatPassword(
      passwordInput.value,
      repeatPasswordInput.value
    );

    if (!validatedRepeatPassword.isValid) {
      repeatPasswordLabel.innerHTML = validatedRepeatPassword.message;
    } else {
      repeatPasswordLabel.innerHTML = "";
    }
  });

  form.addEventListener("submit", (e) => {
    e.preventDefault();

    // One last validation before post, return user object if successful.
    const user = validateRegisterForm(
      emailInput.value,
      emailLabel,
      nameInput.value,
      nameLabel,
      passwordInput.value,
      passwordLabel,
      repeatPasswordInput.value,
      repeatPasswordLabel
    );

    if (user === undefined) {
      return;
    }

    // if user is not undefined, proceed with api call to backend.
    fetch("https://localhost:7264/api/users/register", {
      method: "POST",
      body: JSON.stringify(user),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    }).then(async (response) => {
      const data = await parseResponse(response);

      if (response.ok) {
        successMessage.innerHTML = data;
        errorMessage.innerHTML = "";
        emailInput.value = "";
        nameInput.value = "";
        passwordInput.value = "";
        repeatPasswordInput.value = "";
      } else {
        if (Array.isArray(data.errors)) {
          const messages = data.errors.map((err) => err).join("<br>");
          errorMessage.innerHTML = messages;
        } else if (typeof data === "string") {
          errorMessage.innerHTML = data;
        } else {
          errorMessage.innerHTML = "Ett okänt fel inträffade.";
        }
        successMessage.innerHTML = "";
      }
    });
  });
};

const validateRegisterForm = (
  email,
  emailLabel,
  name,
  nameLabel,
  password,
  passwordLabel,
  repeatPassword,
  repeatPasswordLabel
) => {
  const validatedEmail = validateEmail(email);
  const validatedName = validateName(name);
  const validatedPassword = validatePassword(password);
  const validatedRepeatPassword = validateRepeatPassword(
    password,
    repeatPassword
  );

  if (!validatedEmail.isValid) {
    emailLabel.innerHTML = validatedEmail.message;
    return;
  }
  if (!validatedName.isValid) {
    nameLabel.innerHTML = validatedName.message;
    return;
  }
  if (!validatedPassword.isValid) {
    passwordLabel.innerHTML = validatedPassword.message;
    return;
  }
  if (!validatedRepeatPassword.isValid) {
    repeatPasswordLabel.innerHTML = validatedRepeatPassword.message;
    return;
  }
  return { Email: email, Username: name, Password: password };
};

const validateEmail = (email) => {
  email = email.trim();
  const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  if (email.length === 0) {
    return { isValid: false, message: "En epost krävs" };
  } else if (!emailRegex.exec(email)) {
    return { isValid: false, message: "Ange en giltig epost" };
  }

  return { isValid: true, message: "" };
};

const validateName = (name) => {
  name = name.trim();
  const nameRegex = /^[a-zA-Z]\w{1,30}$/;

  if (name.length === 0) {
    return { isValid: false, message: "Användarnamn får inte vara tomt" };
  } else if (!nameRegex.exec(name)) {
    return {
      isValid: false,
      message:
        "Användarnamn får endast innehålla bokstäver, siffror och understreck",
    };
  }
  return { isValid: true, message: "" };
};

const validatePassword = (password) => {
  password = password.trim();
  const passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).+$/;

  if (password.length < 8 || !passwordRegex.exec(password)) {
    return {
      isValid: false,
      message:
        "Lösenordet måste innehålla minst 8 tecken, en liten bokstav, en stor bokstav, ett specialtecken samt en siffra.",
    };
  }
  return { isValid: true, message: "" };
};

const validateRepeatPassword = (password, repeatPassword) => {
  password = password.trim();
  repeatPassword = repeatPassword.trim();

  if (password !== repeatPassword) {
    return { isValid: false, message: "Lösenorden måste matcha" };
  }
  return { isValid: true, message: "" };
};
