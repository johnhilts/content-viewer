import React, { Component } from 'react';

const renderButton = (content, onClick) => {
    return (
        <button 
            type='button'
            className='link-button' 
            onClick={onClick}>
            {content}
        </button>
    )
}

const renderFolders = (content, contentName, onContentClick) => {
    return (
        <tr key={content.name}>
          <td>
            {renderButton(content.name, onContentClick.bind(null, contentName, content.contentType, null, null))}
          </td>
        </tr>
    )
}

const renderFiles = (content, contentName, onContentClick, currentFolder, currentFileIndex, showThumbnails, onRemoveClick, rotateDegrees) => {
    const photoContainer = document.getElementById('photoContainer')
    const maxHeight = photoContainer ? photoContainer.clientHeight : 800
    const maxWidth = photoContainer ? photoContainer.clientWidth : 1000
    const contentClass = showThumbnails ? 'thumbnail' : 'responsiveImageLarge'
    const isImage = !contentName.endsWith('.mov')
    const imageStyle = rotateDegrees 
        ? {transform: `rotate(${rotateDegrees}deg)`, maxHeight: `${maxHeight}px`, maxWidth: 'auto', marginTop: '15%', }
        : {transform: `rotate(${rotateDegrees}deg)`, }
    const image = <img src={`content/${currentFolder}/${contentName}`} alt={contentName} className={contentClass} style={imageStyle} />
    const imageContainerStyle = rotateDegrees
        ? {height: `${maxHeight + 200}px`}
        : {height: 'auto'}
    const video = <video src={`content/${currentFolder}/${contentName}`} title={contentName} className={contentClass} controls />
    const renderElementType = isImage ? image : video
    const onClick = (fileIndex, rotateDegrees) => onContentClick.bind(null, contentName, content.contentType, fileIndex, rotateDegrees)
    const renderElement = renderButton(renderElementType, onClick(currentFileIndex))
    const renderPrevious = showThumbnails 
        ? <span>&nbsp;</span> 
        : renderButton(<span className='spacer'><i class="fas fa-arrow-left"></i></span>, onClick(currentFileIndex-1))
    const renderNext = showThumbnails 
        ? <div><div style={{marginLeft:'10px'}}>{content.name}</div><div>{content.created}</div><div>{content.geoCoordinateText}</div></div> 
        : renderButton(<span className='spacer'><i class="fas fa-arrow-right"></i></span>, onClick(currentFileIndex+1))
    const removeStyle = showThumbnails ? 'spacer' : 'centeredSpacer'
    const details = showThumbnails ? <span>&nbsp;</span> : <div style={{textAlign:'center'}}><div>{content.created}</div><div>{content.geoCoordinateText}</div></div>

    return (
        <tr key={content.name}>
          <td>
            <div id='photoContainer' style={imageContainerStyle}>
                {renderPrevious}
                {renderElement}
                {renderNext}
            </div>
            {details}
            <div className={removeStyle}>
                <span className={removeStyle}>{renderButton(<i class="fas fa-redo-alt"></i>, onClick(currentFileIndex, 90))}</span>
                <span className={removeStyle}><button onClick={onRemoveClick.bind(null, contentName)}><i class="fas fa-trash-alt"></i></button></span>
                <span className={removeStyle}>{renderButton(<i class="fas fa-undo"></i>, onClick(currentFileIndex, -90))}</span>
            </div>
          </td>
        </tr>
    )
}

const renderContentTable = (contentInfo, onContentClick, currentFolder, currentFileIndex, showThumbnails, onRemoveClick, rotateDegrees) => {
      const getContent = (contentInfo) => {
          const isFolder = contentInfo[0].contentType === 0
          const headerTitle = isFolder
          ? 'Folders'
          : 'Files'
          const viewContentInfo = currentFileIndex === -1 
          ? contentInfo
          : contentInfo.filter((f,i) => i === currentFileIndex)
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                  <tr>
                    <th>{headerTitle}</th>
                  </tr>
                </thead>
                <tbody>
                  {viewContentInfo.map(content => {
                          const contentName = content.name === 'Go Back ...'
                          ? '..'
                          : content.name
                      return isFolder
                      ? renderFolders(content, contentName, onContentClick)
                      : renderFiles(content, contentName, onContentClick, currentFolder, currentFileIndex, showThumbnails, onRemoveClick, rotateDegrees)
                      }
                  )}
                </tbody>
            </table>
        )
      }

    const contents = contentInfo.length
      ?
      getContent(contentInfo)
      :
      <div>&nbsp;</div>

    return contents
  }

    const addReturnFolderData = (folderData) => {
        const folders = folderData ? folderData : []
        return [{name: 'Go Back ...', contentType: 0}, ...folders]
    }

export default function ContentViewer(props) {
    let folderData = addReturnFolderData(props.folders)
    let folders = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(folderData, props.onContentClick, props.currentFolder, props.currentFileIndex, props.showThumbnails);

    let files = props.loading
      ? <p><em>Loading...</em></p>
      : renderContentTable(props.files, props.onContentClick, props.currentFolder, props.currentFileIndex, props.showThumbnails, props.onRemoveClick, props.rotateDegrees);

    return (
      <div>
        <h1 id="tabelLabel" >Content Viewer</h1>
        <p>Select an item to view its content:</p>
        {folders}
        {files}
      </div>
    );
  }
