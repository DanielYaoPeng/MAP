var taskInit = {
    name: "", //内容为空
    id: uuid(),
    date: " Just Now ",
    finished: false, //未完成
    deleted: false //未删除
};



//  extend 创建组件
var footerComponent = Vue.extend({
    template: ` <div id="footer-vue">
                  <p>{{time}} <a href="#">YP's Blog</a> - Hosted by <a href="#" style="font-weight: bold">Coding Pages</a></p>
                  <p>
                      <a  href="#">沪ICP备00000000号{{fooPro}}</a>
                  </p>
              </div>`,
    props: ['fooPro'],
    data() { //这里定义的时模板里"{{}}"需要获取的数据
        return {
            message: "hello world",
            time: getNowYear()
        }
    }
});

// component注册 组件
Vue.component('footer-vue', footerComponent);

//在 vue 的整个生命周期内，总共分为8个阶段创建前/后，载入前/后，更新前/后，销毁前/后。

let vm = new Vue({
    el: "#app",
    data: {
        name: "孙悟猪",
        author: "孙悟猪",
        task: taskInit,
        isShow: true,
        hrClass: 'hr',
        testClass: 'test',
        list: [
            //假数据
            {
                name: " Vue前篇：ES6初体验 & 模块化编程",
                id: 9585766,
                date: "2018年9月5日",
                finished: false,
                deleted: true
            },
            {
                name: "Vue前篇：JS对象&字面量&this",
                id: 9580807,
                date: "2018年9月4日",
                finished: false,
                deleted: false
            },
            {
                name: " VUE 计划书 & 我的前后端开发简史",
                id: 9577805,
                date: "2018年9月3日",
                finished: false,
                deleted: false
            },
            {
                name: " DTOs 对象映射使用，项目部署Windows+Linux完整版",
                id: 3800,
                date: "2018年9月1日",
                finished: false,
                deleted: false
            },
            {
                name: " 三种跨域方式比较，DTOs(数据传输对象)初探",
                id: 4200,
                date: "2018年8月31日",
                finished: false,
                deleted: false
            },
            {
                name: "VUE 计划书 & 我的前后端开发简史",
                id: 3200,
                date: "2018年9月2日",
                finished: false,
                deleted: false
            },
            {
                name: "VUE 实战预告",
                id: 3200,
                date: "2018年9月12日",
                finished: false,
                deleted: false
            }
        ],
        list3: [
            //假数据
            {
                name: "Html5"
            },
            {
                name: "CSS3"
            },
            {
                name: "JAVASCTIPT"
            },
            {
                name: "SQL"
            },
            {
                name: "C#"
            },
            {
                name: "Wechat小程序"
            },
            {
                name: "ASP.NET"
            }
        ],
    },

    components: {
        'mycomponent': {
            template: `<ul class ="contact-list non-style-list">
                  <li>
                      <b class ="twitter">TWITTER</b>: <a href="#">@DanielYao</a>
                  </li>
                  <li>
                      <b class ="weibo">微博</b>: <a href="#">@DanielYao</a>
                  </li>
                  <li>
                      <b class ="zhihu">知乎</b>: <a href="#" ></a>
                  </li>
                  <li>
                      <b class ="github">GITHUB</b>:  <a href="https://github.com/">anjoy8</a>
                  </li>
                  <li>
                      <b class ="email">EMAIL</b>:  <a href="www.mingdao.com">randypriv at YP</a>
                  </li>
              </ul>`,
            data() {
                return {
                    message: 'hello world two'
                }
            },
            directives: {
                focus: {
                    inserted(el) {

                        el.focus();

                    }
                }
            }
        }
    },

    beforeCreate() {
        console.group('beforeCreate 创建前状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el); //undefined
        console.log("%c%s", "color:red", "data   : " + this.$data); //undefined 
        console.log("%c%s", "color:red", "author: " + this.author); //undefined 
        debugger;
    },
    created: function() {
        console.group('created 创建完毕状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el); //undefined
        console.log("%c%s", "color:red", "data   : " + this.$data); //已被初始化 
        console.log("%c%s", "color:red", "author: " + this.author); //已被初始化
        debugger;
    },
    beforeMount: function() {
        //我们知道DOM还没生成，属性el还为 undefined，那么，此阶段为即将挂载，页面渲染成功，el 已经赋值
        console.group('beforeMount 挂载前状态===============》');
        console.log("%c%s", "color:red", "el     : " + (this.$el)); //已被初始化
        console.log(this.$el);
        console.log("%c%s", "color:red", "data   : " + this.$data); //已被初始化  
        console.log("%c%s", "color:red", "author: " + this.author); //已被初始化  
        debugger;
    },
    mounted: function() {
        //挂载完毕阶段，到了这个阶段，数据就会被成功渲染出来
        console.group('mounted 挂载结束状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el); //已被初始化
        console.log(this.$el);
        console.log("%c%s", "color:red", "data   : " + this.$data); //已被初始化
        console.log("%c%s", "color:red", "author: " + this.author); //已被初始化 
        debugger;
    },
    beforeUpdate: function() {
        //更新前（修改Data，但未渲染至页面）
        console.group('beforeUpdate 更新前状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el);
        console.log(this.$el);
        console.log("%c%s", "color:red", "data   : " + this.$data);
        console.log("%c%s", "color:red", "author: " + this.author);
        debugger; //打断点
    },
    updated: function() {
        //更新数据，页面渲染
        console.group('updated 更新完成状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el);
        console.log(this.$el);
        console.log("%c%s", "color:red", "data   : " + this.$data);
        console.log("%c%s", "color:red", "author: " + this.author);
        debugger;
    },
    beforeDestroy: function() {
        //调用实例的destroy( )方法可以销毁当前的组件，在销毁前，会触发beforeDestroy钩子。
        console.group('beforeDestroy 销毁前状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el);
        console.log(this.$el);
        console.log("%c%s", "color:red", "data   : " + this.$data);
        console.log("%c%s", "color:red", "author: " + this.author);
        debugger;
    },
    destroyed: function() {
        //成功销毁之后，会触发 destroyed 钩子，此时该实例与其他实例的关联已经被清除，它与视图之间也被解绑，控制 Data 已经不能控制页面，也无法双向绑定。
        console.group('destroyed 销毁完成状态===============》');
        console.log("%c%s", "color:red", "el     : " + this.$el);
        console.log(this.$el);
        console.log("%c%s", "color:red", "data   : " + this.$data);
        console.log("%c%s", "color:red", "author: " + this.author);
        debugger;
    },
    methods: {
        addArticle() {
            this.list = this.list.reverse();
            this.list.push(this.task);
            this.list = this.list.reverse();
            this.task = initTask();
        },
        removeArticle(index) {
            // this.list.
            //console.log(index);
            this.list.splice(index, 1);
        }
    },
    //通过计算属性过滤数据
    computed: {
        listSearch: function() {

            let that = this; //filter过滤器会改变this的指向 ，保存起来
            return this.list.filter(function(item) {
                //判断以后数据是否和输入相同，筛选功能
                return item.name.toLowerCase().indexOf(that.$data.task.name.toLowerCase()) >= 0;
            });
        },
        nameCpt() {
            return this.task.name
        }
    },
    watch: {
        nameCpt() {
            //这里面可以执行一旦监听的值发生变化你想做的操作
            console.log("变啦变啦！")
        },
        deep: true //表示监视对象的属性变化
    },
});


//函数声明（定义和使用没有先后之分）
function initTask(name = "") {
    return {
        name: name, //内容为空
        id: 100,
        date: " Just Now ",
        finished: false, //未完成
        deleted: false //未删除
    }
};

//guid
function uuid() {
    var s = [];
    var hexDigits = "0123456789abcdef";
    for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4"; // bits 12-15 of the time_hi_and_version field to 0010
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1); // bits 6-7 of the clock_seq_hi_and_reserved to 01
    s[8] = s[13] = s[18] = s[23] = "-";

    var uuid = s.join("");
    return uuid;
}

function getNowYear() {
    var now = new Date();
    return now.getFullYear(); //得到年份
}


