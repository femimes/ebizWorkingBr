<%@ Page Title="" Language="C#" MasterPageFile="~/WebRegisterMain.master" AutoEventWireup="true" CodeFile="WebPages.aspx.cs" Inherits="WebPages" %>

<asp:Content ID="HeadBlock" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .shell {
            display: grid;
            grid-template-columns: 260px 1fr;
            height: 100%;
        }

        /* ---------- Sidebar ---------- */
        .sidenav {
            overflow-y: auto;
            padding: 20px 14px;
            color: #fff;
            background: linear-gradient(180deg, var(--brand) 0%, #082c4d 100%);
        }

        .nav-group { margin-bottom: 8px; }

        .nav-group > summary {
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 8px 12px;
            font-size: 13px;
            font-weight: 700;
            color: rgba(255, 255, 255, .65);
            cursor: pointer;
            list-style: none;
            border-radius: 8px;
        }
        .nav-group > summary::-webkit-details-marker { display: none; }
        .nav-group > summary::after {
            content: "";
            width: 8px;
            height: 8px;
            border-right: 2px solid currentColor;
            border-bottom: 2px solid currentColor;
            transform: rotate(45deg);
            transition: transform .15s ease;
        }
        .nav-group[open] > summary::after { transform: rotate(-135deg); }
        .nav-group > summary:hover { background: rgba(255, 255, 255, .08); }
        .nav-group > summary:focus-visible { outline: 2px solid #fff; outline-offset: 1px; }

        .nav-link {
            display: block;
            margin: 2px 0;
            padding: 10px 12px;
            font-size: 15px;
            font-weight: 500;
            color: rgba(255, 255, 255, .9);
            text-decoration: none;
            border-radius: 8px;
            border-left: 3px solid transparent;
            transition: background-color .15s ease, color .15s ease;
        }
        .nav-link:hover { background: rgba(255, 255, 255, .1); color: #fff; }
        .nav-link:focus-visible { outline: 2px solid #fff; outline-offset: 1px; }
        .nav-link.is-active {
            font-weight: 600;
            color: #fff;
            background: rgba(255, 255, 255, .16);
            border-left-color: #fff;
        }

        /* ---------- Content ---------- */
        .content-frame {
            display: block;
            width: 100%;
            height: 100%;
            border: 0;
            background: var(--page);
        }

        .menu-toggle { display: none; }

        /* ---------- Small screens ---------- */
        @media (max-width: 800px) {
            .shell {
                grid-template-columns: 1fr;
                grid-template-rows: auto 1fr;
            }

            .menu-toggle {
                display: block;
                width: 100%;
                padding: 12px 16px;
                font: inherit;
                font-size: 15px;
                font-weight: 600;
                text-align: left;
                color: var(--brand);
                background: var(--surface);
                border: 0;
                border-bottom: 1px solid var(--line);
                cursor: pointer;
            }
            .menu-toggle:focus-visible { outline: none; box-shadow: inset 0 0 0 3px var(--ring); }

            .sidenav {
                display: none;
                max-height: 50vh;
            }
            .sidenav.is-open { display: block; }

            /* menu button sits in row 1 with the nav; content in row 2 */
            .shell > .menu-toggle,
            .shell > .sidenav { grid-column: 1; }
            .shell > .content-frame { grid-column: 1; grid-row: 2; margin:2%;}
            .shell > .menu-toggle { grid-row: 1; }
            .shell > .sidenav { grid-row: 1; margin-top: 46px; position: absolute; left: 0; right: 0; z-index: 5; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="shell">

        <button type="button" id="menuToggle" class="menu-toggle" aria-expanded="false" aria-controls="sidenav">Menu</button>

        <nav id="sidenav" class="sidenav" aria-label="Main navigation">

            <details class="nav-group" open>
                <summary>Manage Account</summary>

                <a class="nav-link is-active" href="Processing/HomeDefault.aspx" target="contentUrlPane" aria-current="page"
                    title="Subscribe to new products and view all existing payment details">Home</a>

                <%-- Hidden for now:
                <a class="nav-link" href="Subscription/CustomerProfile.aspx?z=0" target="contentUrlPane" title="Change profile">Edit Profile</a>
                <a class="nav-link" href="Processing/ChangePwdsOnly.aspx" target="contentUrlPane" title="Change my password only">Change Password</a>
                --%>
            </details>

        <%-- Groups and items below come from the database (see LoadSubMenus in WebPages.aspx.cs) --%>
            <asp:Literal ID="litMenu" runat="server" />

        </nav>

        <iframe id="contentUrlPane" name="contentUrlPane" class="content-frame" 
            src="Processing/HomeDefault.aspx" title="Page content"  ></iframe>

    </div>

    <script type="text/javascript">
        (function () {
            var nav = document.getElementById('sidenav');
            var toggle = document.getElementById('menuToggle');
            var links = nav.querySelectorAll('.nav-link');

            // Highlight the page the person is on
            for (var i = 0; i < links.length; i++) {
                links[i].addEventListener('click', function () {
                    for (var j = 0; j < links.length; j++) {
                        links[j].classList.remove('is-active');
                        links[j].removeAttribute('aria-current');
                    }
                    this.classList.add('is-active');
                    this.setAttribute('aria-current', 'page');

                    // Close the menu after choosing on small screens
                    nav.classList.remove('is-open');
                    toggle.setAttribute('aria-expanded', 'false');
                });
            }

            // Keep one group open at a time, like the old AutoCollapse
            var groups = nav.querySelectorAll('details.nav-group');
            for (var g = 0; g < groups.length; g++) {
                groups[g].addEventListener('toggle', function () {
                    if (!this.open) { return; }
                    for (var k = 0; k < groups.length; k++) {
                        if (groups[k] !== this) { groups[k].open = false; }
                    }
                });
            }

            toggle.addEventListener('click', function () {
                var open = nav.classList.toggle('is-open');
                toggle.setAttribute('aria-expanded', open);
            });
        })();
    </script>

</asp:Content>
