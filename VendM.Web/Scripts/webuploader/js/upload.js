
(function ($, window) {
    //文件上传
    var UrlList = [];
    function initWebUpload(item, options) {
        var GUID = WebUploader.Base.guid();
        var $wrap = $(item),
            // 图片容器
            $queue = $('<ul class="filelist"></ul>').appendTo($wrap.find('.queueList')),
            // 状态栏，包括进度和控制按钮
            $statusBar = $wrap.find('.statusBar'),
            // 文件总体选择信息。
            $info = $statusBar.find('.info'),
            // 上传按钮
            $upload = $wrap.find('.uploadBtn'),
            // 没选择文件之前的内容。
            $placeHolder = $wrap.find('.placeholder'),
            $progress = $statusBar.find('.progress').hide(),
            // 添加的文件数量
            fileCount = 0,
            // 添加的文件总大小
            fileSize = 0,
            // 优化retina, 在retina下这个值是2
            ratio = window.devicePixelRatio || 1,
            // 缩略图大小
            thumbnailWidth = 110 * ratio,
            thumbnailHeight = 110 * ratio,

            // 可能有pedding, ready, uploading, confirm, done.
            state = 'pedding',

            // 所有文件的进度信息，key为file id
            percentages = {},
            // 判断浏览器是否支持图片的base64
            isSupportBase64 = (function () {
                var data = new Image();
                var support = true;
                data.onload = data.onerror = function () {
                    if (this.width != 1 || this.height != 1) {
                        support = false;
                    }
                }
                data.src = "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw==";
                return support;
            })(),
            // 检测是否已经安装flash，检测flash的版本
            flashVersion = (function () {
                var version;

                try {
                    version = navigator.plugins['Shockwave Flash'];
                    version = version.description;
                } catch (ex) {
                    try {
                        version = new ActiveXObject('ShockwaveFlash.ShockwaveFlash')
                            .GetVariable('$version');
                    } catch (ex2) {
                        version = '0.0';
                    }
                }
                version = version.match(/\d+/g);
                return parseFloat(version[0] + '.' + version[1], 10);
            })(),

            supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                        'WebkitTransition' in s ||
                        'MozTransition' in s ||
                        'msTransition' in s ||
                        'OTransition' in s;
                s = null;
                return r;
            })(),
            // WebUploader实例
            uploader;
        var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../../";
        // 实例化
        var defult = {
            pick: {
                id: '#filePicker',
                label: options.label
            },
            formData: {
                uid: GUID
            },
            dnd: '#uploader .queueList',
            paste: '#uploader',
            swf: applicationPath + '../Content/scripts/plugins/webuploader/Uploader.swf',
            server: applicationPath + options.upServerUrl,
            // 禁掉全局的拖拽功能。这样不会出现图片拖进页面的时候，把图片打开。
            disableGlobalDnd: true,
            fileNumLimit: options.fileNumLimit,
            fileSizeLimit: options.fileSizeLimit,
            fileSingleSizeLimit: options.fileSingleSizeLimit,
        }
        if (options.isBigFile) {
            defult.chunked = true;//开始分片上传
            defult.chunkSize = 2048000;//每一片的大小
            defult.formData = { guid: GUID };////自定义参数，待会儿解释
        }
        uploader = WebUploader.create(defult);
        // 拖拽时不接受 js, txt 文件。
        uploader.on('dndAccept', function (items) {
            var denied = false,
                len = items.length,
                i = 0,
                // 修改js类型
                unAllowed = 'text/plain;application/javascript ';
            for (; i < len; i++) {
                // 如果在列表里面
                if (~unAllowed.indexOf(items[i].type)) {
                    denied = true;
                    break;
                }
            }
            return !denied;
        });

        uploader.on('dialogOpen', function () {

        });

        function getUrlBase64(url, ext, callback) {
            var canvas = document.createElement("canvas");   //创建canvas DOM元素
            var ctx = canvas.getContext("2d");
            var img = new Image;
            img.crossOrigin = 'Anonymous';
            img.src = url;
            img.onload = function () {
                canvas.height = 60; //指定画板的高度,自定义
                canvas.width = 85; //指定画板的宽度，自定义
                ctx.drawImage(img, 0, 0, 60, 85); //参数可自定义
                var dataURL = canvas.toDataURL("image/" + ext);
                callback.call(this, dataURL); //回掉函数获取Base64编码
                canvas = null;
            };
        };
        // 添加“添加文件”的按钮，
        uploader.addButton({
            id: '#filePicker2',
            label: '继续添加'
        });
        uploader.on('ready', function () {
            window.uploader = uploader;
        });
        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {
            var $li = $('<li id="' + file.id + '">' +
                '<p class="title">' + file.name + '</p>' +
                '<p class="imgWrap"></p>' +
                '<p class="progress"><span></span></p>' +
                '<a data-url="url" value="' + file.name + '"></li >'),

                $btns = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span>' +
                    '<span class="rotateRight">向右旋转</span>' +
                    '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
                $prgress = $li.find('p.progress span'),
                $wrap = $li.find('p.imgWrap'),
                $info = $('<p class="error"></p>'),

                showError = function (code) {
                    switch (code) {
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;

                        case 'interrupt':
                            text = '上传暂停';
                            break;

                        default:
                            text = '上传失败，请重试';
                            break;
                    }

                    $info.text(text).appendTo($li);
                };

            if (file.getStatus() === 'invalid') {
                showError(file.statusText);
            } else {
                $wrap.text('预览中');
                uploader.makeThumb(file, function (error, src) {
                    var img;
                    if (error) {
                        $wrap.text('不能预览');
                        return;
                    }
                    if (isSupportBase64) {
                        img = $('<img src="' + src + '">');
                        $wrap.empty().append(img);
                    }
                }, thumbnailWidth, thumbnailHeight);

                percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function (cur, prev) {
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    $li.off('mouseenter mouseleave');
                    $btns.remove();
                }

                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    showError(file.statusText);
                    percentages[file.id][1] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    $info.remove();
                    $prgress.css('display', 'block');
                    percentages[file.id][1] = 0;
                } else if (cur === 'progress') {
                    $info.remove();
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    $prgress.hide().width(0);
                    $li.append('<span class="success"></span>');
                }

                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });

            $li.on('mouseenter', function () {
                $btns.stop().animate({ height: 30 });
            });

            $li.on('mouseleave', function () {
                $btns.stop().animate({ height: 0 });
            });

            $btns.on('click', 'span', function () {
                var index = $(this).index(),
                    deg;
                switch (index) {
                    case 0:
                        uploader.removeFile(file);
                        return;

                    case 1:
                        file.rotation += 90;
                        break;

                    case 2:
                        file.rotation -= 90;
                        break;
                }
                if (supportTransition) {
                    deg = 'rotate(' + file.rotation + 'deg)';
                    $wrap.css({
                        '-webkit-transform': deg,
                        '-mos-transform': deg,
                        '-o-transform': deg,
                        'transform': deg
                    });
                } else {
                    $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                }
            });
            $li.appendTo($queue);
        }

        // 负责view的销毁
        function removeFile(file) {
            if (options.type == 'add') {
                var $li = $('#' + file.id);
                delete percentages[file.id];
                updateTotalProgress();
                $li.off().find('.file-panel').off().end().remove();
            } else {
                deleteFile(file);
            }
        }
        function deleteFile(file) {
            var filename = file.name;
            if (filename == undefined || filename === "")
                return;
            $.ajax({
                type: 'get',
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                async: true,
                url: options.deleteServerUrl + '&filename=' + filename,
                success: function (result) {
                    if (result.Success) {
                        layer.closeAll('dialog');
                        layer.msg('删除成功！',
                            {
                                time: 2000
                            });
                        var $li = $('#' + file.id);
                        delete percentages[file.id];
                        updateTotalProgress();
                        $li.off().find('.file-panel').off().end().remove();
                    } else {
                        layer.open({
                            title: '提示',
                            content: '删除失败！',
                            move: false,
                            btn: "知道了"
                        });
                    }
                },
                error: function (result) {
                    console.log(result);
                }
            });
        }
        function updateTotalProgress() {
            var loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(percentages, function (k, v) {
                total += v[0];
                loaded += v[0] * v[1];
            });
            percent = total ? loaded / total : 0;
            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        }

        function updateStatus() {
            var text = '', stats;
            if (state === 'ready') {
                text = '选中' + fileCount + '张图片，共' +
                    WebUploader.formatSize(fileSize) + '。';
            } else if (state === 'confirm') {
                stats = uploader.getStats();
                if (stats.uploadFailNum) {
                    text = '已成功上传' + stats.successNum + '张照片，' +
                        stats.uploadFailNum + '张照片上传失败，<a class="retry" href="#">重新上传</a>失败图片或<a class="ignore" href="#">忽略</a>'
                }
            } else {
                stats = uploader.getStats();
                text = '共' + fileCount + '张（' +
                    WebUploader.formatSize(fileSize) +
                    '），已上传' + stats.successNum + '张';

                if (stats.uploadFailNum) {
                    text += '，失败' + stats.uploadFailNum + '张';
                }
            }

            $info.html(text);
        }
        function setState(val, file) {
            var file, stats;
            if (val === state) {
                return;
            }

            $upload.removeClass('state-' + state);
            $upload.addClass('state-' + val);
            state = val;

            switch (state) {
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;
                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    $('#filePicker2').removeClass('element-invisible');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'uploading':
                    $('#filePicker2').addClass('element-invisible');
                    $progress.show();
                    $upload.text('暂停上传');
                    break;
                case 'paused':
                    $progress.show();
                    $upload.text('继续上传');
                    break;
                case 'confirm':
                    $progress.hide();
                    $('#filePicker2').removeClass('element-invisible');
                    $upload.text('开始上传');

                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        layer.closeAll('dialog');
                        layer.msg('上传成功！',
                            {
                                time: 2000
                            });
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }
            updateStatus();
        }

        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

            $percent.css('width', percentage * 100 + '%');
            percentages[file.id][1] = percentage;
            updateTotalProgress();
        };
        uploader.onFileQueued = function (file) {
            fileCount++;
            fileSize += file.size;

            if (fileCount === 1) {
                $placeHolder.addClass('element-invisible');
                $statusBar.show();
            }

            addFile(file);
            setState('ready');
            updateTotalProgress();
        };

        uploader.onFileDequeued = function (file) {
            fileCount--;
            fileSize -= file.size;

            if (!fileCount) {
                setState('pedding');
            }

            removeFile(file);
            updateTotalProgress();

        };

        uploader.on('all', function (type) {
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    break;
                case 'startUpload':
                    setState('uploading');
                    break;
                case 'stopUpload':
                    setState('paused');
                    break;
                case 'uploadSuccess':
                    setState('paused');
                    break;
                case 'uploadFinished':
                    break;
            }
        });
        uploader.on('uploadSuccess', function (file, response) {
            $('#' + file.id).find('p.state').text('已上传');
            debugger;
            if (options.isBigFile) {
                $.post(options.upPartUrl, { guid: GUID, fileName: file.name }, function (data) {
                    if (data.Success) {
                        $('#' + file.id).find('a').attr("value", data.Data)
                        $("#uploader .state").html("上传成功...");
                    } else {
                        $("#uploader .state").html("上传失败...");
                    }
                });
            } else {
                $('#' + file.id).find('a').attr("value", response.Data)
            }
        });
        //所有文件上传完毕
        uploader.on("uploadFinished", function () {
            //  return UrlList; 
           // $("#" + options.fileId + "").val(UrlList.join(','));
        });

        uploader.onError = function (code) {
            alert('Eroor: ' + code);
        };

        //需要编辑的图片列表
        var blobToFile = function (blob, name) {
            blob.lastModifiedDate = new Date();
            blob.name = name;
            return blob;
        };
        var getFileObject = function (filePathOrUrl, cb) {
            getFileBlob(filePathOrUrl, function (blob) {
                cb(blobToFile(blob, filePathOrUrl));
            });
        };
        var getFileBlob = function (url, cb) {
            var xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.responseType = "blob";
            xhr.addEventListener('load', function () {
                cb(xhr.response);
            });
            xhr.send();
        };
        $.each(options.fileUrl, function (index, item) {
            getFileObject(item,
                function (fileObject) {
                    var wuFile = new WebUploader.Lib.File(WebUploader.guid('rt_'), fileObject);
                    var file = new WebUploader.File(wuFile);
                    uploader.addFiles(file);
                });
        });

        $upload.on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }
            if (state === 'ready') {
                uploader.upload();
            } else if (state === 'paused') {
                uploader.upload();
            } else if (state === 'uploading') {
                uploader.stop();
            }
        });
        $info.on('click', '.retry', function () {
            uploader.retry();
        });
        $info.on('click', '.ignore', function () {
            alert('todo');
        });
        $upload.addClass('state-' + state);
        updateTotalProgress();
    }

    $.fn.CleanUpload = function (options) {
        var uploadrFile = UpdataLoadarrayObj[$(this).attr("id")]
        var fileslist = uploadrFile.getFiles();
        for (var i in fileslist) {
            uploadrFile.removeFile(fileslist[i]);
        }
    }
    $.fn.GetFilesAddress = function (options) {
        //var ele = $(this);
        //var filesdata = ele.find(".UploadhiddenInput");
        //var filesAddress = [];
        //filesdata.find(".hiddenInput").each(function () {
        //    filesAddress.push($(this).val());
        //})
        //return filesAddress;
        return UrlList;
    }

    $.fn.WebUpload = function (options) {
        var ele = this;
        initWebUpload(ele, options);
    }
})(jQuery, window);