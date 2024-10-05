// util function just for syntax highlighting
// you can download the `lit-html` vscode extension to enabled the syntax highlight
// https://marketplace.visualstudio.com/items?itemName=bierner.lit-html
function html(strings, ...values) {
  return strings.reduce(
    (result, str, i) => result + str + (values[i] || ""),
    ""
  );
}

// Cookie 操作函数
//function getCookie(name) {
//    const value = `; ${document.cookie}`;
//    const parts = value.split(`; ${name}=`);
//    if (parts.length === 2) return parts.pop().split(';').shift();
//}

//function setCookie(name, value, days) {
//    let expires = "";
//    if (days) {
//        const date = new Date();
//        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
//        expires = `; expires=${date.toUTCString()}`;
//    }
//    document.cookie = `${name}=${value || ""}${expires}; path=/`;
//}

const HeaderComponent = {
  template: html`
    <nav class="navbar navbar-expand-lg navbar-custom fixed-top">
      <div class="container-fluid">
        <a href="index.html" class="col-1">
          <img src="image/logo1.jpg" alt="首頁" class="img-fluid rounded" />
        </a>
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav col-6">
            <li class="nav-item col-3">
              <a
                class="nav-link"
                :class="{'active-link': isActiveLink('News.html')}"
                href="News.html"
                >最新消息</a
              >
            </li>
            <li class="nav-item col-3">
              <a
                class="nav-link"
                :class="{'active-link': isActiveLink('Hots.html')}"
                href="Hots.html"
                >熱門商品</a
              >
            </li>
            <li class="nav-item col-3">
              <a
                class="nav-link"
                :class="{'active-link': isActiveLink('PC.html')}"
                href="PC.html"
                >電腦專區</a
              >
            </li>
            <li class="nav-item col-3">
              <a
                class="nav-link"
                :class="{'active-link': isActiveLink('Devices.html')}"
                href="Devices.html"
                >週邊專區</a
              >
            </li>
          </ul>
        </div>

        <form
          class="d-flex align-items-center"
          role="search"
          :action="this.pathname"
          method="get"
        >
          <div class="input-group">
            <span class="input-group-text" id="basic-addon1">
              <i class="bi bi-search"></i>
            </span>
            <input
              class="form-control"
              name="search"
              type="text"
              placeholder="請輸入商品名稱"
              aria-label="Search"
              v-model="searchQuery"
            />
          </div>
          <button class="btn btn-danger ms-2" type="submit">
            <i class="bi bi-search"></i>
          </button>
        </form>

        <div class="d-flex">
          <a
            href="#"
            class="ms-5 btn btn-link"
            @click="handleOrderClick"
            :class="{'active-link': isActiveLink('Order.html')}"
          >
            <i class="bi bi-person-circle"></i>
          </a>
          <a
            :href="isLoggedIn? '#': 'Login.html'"
            class="ms-5 btn btn-link"
            :class="{'active-link': isActiveLink('Login.html')}"
            @click="handleLoginClick"
          >
            <i
              :class="isLoggedIn ? 'bi bi-box-arrow-in-left' : 'bi bi-box-arrow-in-right'"
            ></i>
          </a>
          <a
            href="#"
            class="ms-5 btn btn-link"
            @click="handleCartClick"
            :class="{'active-link': isActiveLink('cart.html')}"
          >
            <i class="bi bi-cart"></i>
          </a>
        </div>
      </div>
    </nav>
  `,
  data() {
    return {
      searchQuery: "", // 绑定搜索输入
      pathname: "",
      isLoggedIn: false,
    };
  },

  mounted() {
    this.pathname = window.location.pathname;
    const loggedInStatus = localStorage.getItem("user");
    if (loggedInStatus) {
      this.isLoggedIn = true;
    } else {
      this.isLoggedIn = false;
    }
  },
  methods: {
    // searchProduct() {
    //   console.log(1, { emit: $emit, search: this.searchQuery });
    //   this.$emit("search", this.searchQuery); // 通过事件传递搜索关键字
    //   // 清空搜索框内容
    //   this.searchQuery = "";
    // },
    isActiveLink(pathname) {
      return window.location.pathname.includes(pathname);
    },
    handleLoginClick() {
      if (this.isLoggedIn) {
        // 用户已登录，点击时显示 SweetAlert 并处理登出逻辑
        localStorage.removeItem("user");
        Swal.fire({
          icon: "success",
          title: "登出成功",
          showConfirmButton: false,
          timer: 1500,
        }).then(() => {
          // this.isLoggedIn = false;
          // setCookie("auth_token", "false", 0); // 删除 Cookie
          setTimeout(() => {
            window.location.reload(); // 刷新页面，确保状态更新
          }, 2000);
        });
      } else {
        // 用户未登录，跳转到登录页面
        window.location.href = "Login.html";
      }
    },
    handleOrderClick() {
      if (this.isLoggedIn) {
        // 如果用户已登录，跳转到订单页面
        window.location.href = "Order.html";
      } else {
        // 未登录时跳转到登录页面
        Swal.fire({
          icon: "info",
          title: "請先登入",
          showConfirmButton: false,
          timer: 1500,
        }).then(() => {
          setTimeout(() => {
            window.location.href = "Login.html";
          }, 2000);
        });
      }
    },
    handleCartClick() {
      if (this.isLoggedIn) {
        // 如果用户已登录，跳转到购物车页面
        window.location.href = "cart.html";
      } else {
        // 未登录时跳转到登录页面
        Swal.fire({
          icon: "info",
          title: "請先登入",
          showConfirmButton: false,
          timer: 1500,
        }).then(() => {
          window.location.href = "Login.html";
        });
      }
    },
  },
};

const FooterComponent = {
  template: `
    <footer>
      <div class="container">
        <div class="row d-flex justify-content-between align-items-start">
          <!-- 公司資訊 -->
          <div class="col-md-4 text-center">
            <h5>關於我們</h5>
            <p>
              Fat4Fun 是一家致力於提供高性能電競設備的公司
              <br />專注於滿足玩家的需求。
            </p>
          </div>

          <!-- 聯絡我們 -->
          <div class="col-md-4 text-center mx-auto">
            <h5>聯絡我們</h5>
            <p class="text-center">
              Email: support@fat4fun.com <br />
              電話: +886 1234-5678
            </p>
          </div>

          <!-- 社交媒體 -->
          <div class="col-md-4 text-center">
            <h5>關注我們</h5>
            <div class="social-icons">
              <a
                href="https://www.facebook.com/profile.php?id=100009114848126"
                class="text-white"
              >
                <i class="bi bi-facebook"></i>
              </a>
              <a
                href="https://www.instagram.com/liaohongming/"
                class="text-white"
              >
                <i class="bi bi-instagram"></i>
              </a>
            </div>
          </div>
        </div>

        <!-- 底部聲明 -->
        <div class="row mt-4 bottom-info">
          <div class="col text-center">
            <p class="mb-0">
              Fat4Fun &copy; 2024. All rights reserved.
              <a href="#">隱私政策</a> | <a href="#">服務條款</a>
            </p>
          </div>
        </div>
      </div>
    </footer>
  `,
};

export { HeaderComponent, FooterComponent };
