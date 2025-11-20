

window.exportDynamicPdf = (data, columns) => {
            const {jsPDF} = window.jspdf;
    const doc = new jsPDF();

    const headers = [columns];
            const rows = data.map(row => columns.map(col => row[col] || ''));

    doc.autoTable({
        head: headers,
    body: rows,
    theme: 'grid',
    headStyles: {fillColor: [33, 150, 243] }, // color azul
            });

    doc.save('exported-table.pdf');
};

    function saveAsFile(filename, bytesBase64) {
            var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};


window.printGrid = (columns, data) => {
    let html = `<table border="1" cellspacing="0" cellpadding="5" style="border-collapse:collapse; width:100%;">`;
    html += `<thead><tr>`;
    columns.forEach(col => {
        html += `<th style="background-color:#007bff; color:white;">${col}</th>`;
    });
    html += `</tr></thead><tbody>`;

    data.forEach(row => {
        html += `<tr>`;
        columns.forEach(col => {
            html += `<td>${row[col] || ''}</td>`;
        });
        html += `</tr>`;
    });

    html += `</tbody></table>`;

    let printWindow = window.open('', '', 'width=900,height=700');
    printWindow.document.write(`<html><head><title>Imprimir datos</title></head><body>${html}</body></html>`);
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
};

