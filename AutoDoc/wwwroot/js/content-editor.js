function initializeTinyMCE(selector, contentValue) {
    console.log('Initialization of editor');

    tinymce.init({
        selector: selector,
        height: 600,
        width: '100%',
        //Used plugins
        plugins: 'advlist autolink lists link image charmap preview anchor searchreplace visualblocks code fullscreen insertdatetime table code help wordcount',
        toolbar: 'undo redo | blocks | forecolor backcolor | formatselect | bold italic fontfamily fontsize | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help',
        menubar: true,
        image_title: true,
        //Disabled promotion of tinymce and upgrade to higher version
        promotion: false,
        branding: false,
        license_key: 'gpl',
        forced_root_block: 'p',
        //Break lines
        force_br_newlines: false,
        convert_newlines_to_brs: false,
        skin: 'oxide',
        content_style: 'body { height: 450px !important;  }',
        // Choose a file from desktop
        file_picker_callback: filePickerCallback,
        //Setup and initialization of editor
        setup: editorSetup(contentValue)
    });
}

function filePickerCallback(cb, value, meta) {
    const input = document.createElement('input');
    input.setAttribute('type', 'file');
    input.setAttribute('accept', 'image/*');

    input.addEventListener('change', (e) => {
        const file = e.target.files[0];

        //Moguce odabrati samo sliku
        if (!file.type.startsWith('image/')) {
            alert('Potrebno odabrati sliku kao tip dokumenta!');
            return;
        }

        //Moguce odabrati samo sliku koja sadrzi 500 KB i manje
        const maxSizeInBytes = 500 * 1024;
        if (file.size >= maxSizeInBytes) {
            alert('Slika prelazi 500 KB. Molimo Vas da odaberete manju sliku.');
            return;
        }

        const reader = new FileReader();
        reader.addEventListener('load', () => {
            const id = 'blobid' + (new Date()).getTime();
            const blobCache = tinymce.activeEditor.editorUpload.blobCache;
            const base64 = reader.result.split(',')[1];
            const blobInfo = blobCache.create(id, file, base64);
            blobCache.add(blobInfo);
            /* call the callback and populate the Title field with the file name */
            cb(blobInfo.blobUri(), { title: file.name });
        });
        reader.readAsDataURL(file);
    });

    input.click();
}

function editorSetup(contentValue) {
    return function (editor) {
        //Event na drop kada se prebaci odredjeni dokument u editor
        //editor.on('Drop', handleFileDrop);

        editor.on('init', function () {
            if (contentValue != null) {
                editor.setContent(contentValue);
            }
        });

        editor.on('dragover', function (e) {
            e.preventDefault();
        });

        editor.on('drop', function (e) {
            if (e.dataTransfer.files.length > 0) {
                handleFileDrop(e);
            } else {
                e.preventDefault();
                const data = e.dataTransfer.getData("text/plain");
                // Check if dataTransfer contains files and only process placeholders if 
                if (e.dataTransfer.files.length === 0 && data) {
                    const placeholderText = `{{${data}}}`;
                    editor.focus();
                    editor.selection.setContent(placeholderText);
                    editor.save();
                }
            }
        });
    };
}

function handleFileDrop(e) {
    const file = e.dataTransfer.files[0];

    if (file) {
        // Provjera da li je tip dokumenta slika
        if (!file.type.startsWith('image/')) {
            alert('Potrebno odabrati sliku kao tip dokumenta!');
            e.preventDefault();
            return;
        }

        // Provjera da li slika prelazi 500 KB
        const maxSizeInBytes = 500 * 1024;
        if (file.size >= maxSizeInBytes) {
            alert('Slika prelazi 500 KB. Molimo Vas da odaberete manju sliku.');
            e.preventDefault();
            return;
        }
    }
}

function getEditorContent(editorId) {
    return tinymce.get(editorId).getContent();
}

function destroyTinyMCE(selector) {
    const existingInstance = tinymce.get(selector);
    if (existingInstance) {
        tinymce.remove(existingInstance);
        console.log('Existing editor instance destroyed');
    }
}

function drag(ev) {
    ev.dataTransfer.setData("text/plain", ev.target.getAttribute("data-placeholder"));
}

function allowDrop(ev) {
    ev.preventDefault();
}

function drop(ev) {
    ev.preventDefault();
    const data = ev.dataTransfer.getData("text/plain");
    const placeholderText = `{{${data}}}`;

    if (tinymce.activeEditor) {
        tinymce.activeEditor.focus();
        tinymce.activeEditor.selection.setContent(placeholderText);
        tinymce.activeEditor.save();
    }
}

function toggleGroup(groupId) {
    console.log('uslo u toggle')
    var group = document.getElementById(groupId);
    if (group.style.display === "none") {
        group.style.display = "block";
    } else {
        group.style.display = "none";
    }
}