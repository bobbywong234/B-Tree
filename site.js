//构建红黑树
function structuring(tree, level) {
    let childrenString = "";
    level += 2;
    console.log(tree.id);
    if (tree.children != null) {
        for (let i = 0; i < tree.children.length; i++) {
            let child = tree.children[i];
            childrenString += '<div id="' + child.id.toString() +
                '" style="padding-left:' + level.toString() + 'em">{Id:' +
                child.id + ', '
                + 'Pid:' + child.pid + ', ' +
                'Data:{' + child.data.idCard + ', ' + child.data.name +  '}, ' +
                'Sort:' + child.sort
                + '}';
            var nextChild = "";

            if (child.children != null) { nextChild = structuring(child, level); }
            childrenString += nextChild;
            childrenString += '</div>'
        }
    }
    return childrenString;
}

//获取红黑树数据
function redblacktreeonload() {
    let buffer;
    $.ajax({
        type: "Get",
        datatype: "json",
        timeout: 20000,
        url: "api/redblacktree",
    }).then((res => {
        buffer = res;
        $("#RedBlackTree").append('<div>红黑树结构 </div>')
        let elementString = '<div id="' + buffer.id.toString() +
            '" style="padding-left:0em">{Id:' +
            buffer.id + ', '
            + 'Pid:' + buffer.pid + ', ' +
            'Data:{' + buffer.data.idCard + ', '+ buffer.data.name + '}, ' +
            'Sort:' + buffer.sort
            + '}';
        elementString += structuring(buffer, -1);
        elementString += '</div>';
        $("#RedBlackTree").append(elementString);
    }));
}