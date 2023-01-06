window.copyElement = (sourceId, copyId) =>
{
    const sourceElement = document.getElementById(sourceId);
    if (sourceElement == null)
        return;
    
    const node = sourceElement?.firstChild;
    const sourceNode = node.cloneNode(true);
    if (sourceNode == null)
        return;
    
    const elem = document.getElementById(copyId + "-inner");

    elem.after(sourceNode);

    for (let childElement of getDescendantNodes(sourceElement)) {
        if (childElement.id === "") continue;

        scroll(childElement, copyId);
    }
}

window.scrollUp = (sourceId) => {
    const sourceElement = document.getElementById(sourceId);
    if (sourceElement == null)
        return;

    for (let element of getDescendantNodes(sourceElement)) {
        if (element.id === "") continue;

        if (typeof element.scroll === "function") {
            element.scroll({ top: 0 });
        }
    }
}

window.removeElement = (copyId) => {
    const sourceElement = document.getElementById(copyId);
    for (const node of sourceElement.childNodes)
        if (node.id !== copyId + "-inner") node.remove();
}

const getDescendantNodes = (node, all = []) => {
    all.push(...node.childNodes);
    for (const child of node.childNodes)
        getDescendantNodes(child, all);
    return all;
}

const scroll = (childElement, copyId) => {
    if (childElement == null || childElement.scrollTop === 0)
        return;

    const copyElement = document.getElementById(copyId);

    if (copyElement == null)
        return;

    const scrollElement = copyElement.querySelector('#'+ childElement.id);

    if (scrollElement == null)
        return;

    scrollElement.scrollTop = childElement.scrollTop;
    scrollElement.scrollLeft = childElement.scrollLeft;
}

window.scrollTo = (x, y) => {
    if (x === 0 && y === 0)
        return;
};
