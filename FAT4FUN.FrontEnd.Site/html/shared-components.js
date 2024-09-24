const HeaderComponent = {
  template: `
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
          <ul class="navbar-nav col-7">
            <li class="nav-item col-3">
              <a class="nav-link" href="#">最新消息</a>
            </li>
            <li class="nav-item col-3">
              <a class="nav-link" href="#">熱門商品</a>
            </li>
            <li class="nav-item col-3">
              <a class="nav-link" href="PC.html">PC專區</a>
            </li>
            <li class="nav-item col-3">
              <a class="nav-link disabled" aria-disabled="true">週邊專區</a>
            </li>
          </ul>
        </div>
        <form class="d-flex align-items-center" role="search">
          <div class="input-group">
            <span class="input-group-text" id="basic-addon1"
              ><i class="bi bi-search"></i
            ></span>
            <input
              class="form-control"
              type="search"
              placeholder="請輸入商品名稱"
              aria-label="Search"
            />
          </div>
          <button class="btn btn-danger ms-2" type="submit">
            <i class="bi bi-search"></i>
          </button>
        </form>

        <div class="d-flex">
          <a href="Member.html" class="ms-5 btn btn-link">
            <i class="bi bi-person-circle"></i>
          </a>
          <a href="Register.html" class="ms-5 btn btn-link">
            <i class="bi bi-box-arrow-in-right"></i>
          </a>
          <a href="Member.html" class="ms-5 btn btn-link">
            <i class="bi bi-cart"></i>
          </a>
        </div>
      </div>
    </nav>
  `
}

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
  `
}
